using System.Collections.Generic;
using UnityEngine;

namespace u_i_1
{
    public static class Timing
    {
        private static LinkedList<CoroutineReplacement1.crValues> coroutine = new LinkedList<CoroutineReplacement1.crValues>();
        private static LinkedList<CoroutineReplacement1.crValues>[] coroutines = new LinkedList<CoroutineReplacement1.crValues>[5]
        {
            new LinkedList<CoroutineReplacement1.crValues>(),
            new LinkedList<CoroutineReplacement1.crValues>(),
            new LinkedList<CoroutineReplacement1.crValues>(),
            new LinkedList<CoroutineReplacement1.crValues>(),
            new LinkedList<CoroutineReplacement1.crValues>()
        };

        public static CoroutineHandle RunCoroutine(IEnumerator<float> cr)
        {
            return new CoroutineHandle()
            {
                bin = coroutine,
                handle = coroutine.AddLast(new CoroutineReplacement1.crValues()
                {
                    remainingTime = 0F,
                    enumerator = cr
                })
            };
        }

        public static CoroutineHandle RunCoroutine(IEnumerator<float> cr, int bin)
        {
            return new CoroutineHandle()
            {
                bin = coroutines[bin],
                handle = coroutines[bin].AddLast(new CoroutineReplacement1.crValues()
                {
                    remainingTime = 0F,
                    enumerator = cr
                })
            };
        }

        public static float WaitForSeconds(float duration)
        {
            return duration;
        }

        public static void KillCoroutines(CoroutineHandle handle)
        {
            handle.bin.Remove(handle.handle);
        }

        public static void KillCoroutines(int bin)
        {
            coroutines[bin] = new LinkedList<CoroutineReplacement1.crValues>();
        }

        public static transfer getCoroutines()
        {
            return new transfer()
            {
                a = coroutine,
                bins = coroutines
            };
        }

        public struct transfer
        {
            public LinkedList<CoroutineReplacement1.crValues> a;
            public LinkedList<CoroutineReplacement1.crValues>[] bins;
        };
    }

    public class CoroutineReplacement1 : MonoBehaviour
    {
        public struct crValues
        {
            public float remainingTime;
            public IEnumerator<float> enumerator;
        };

        private LinkedList<crValues> coroutine;
        private LinkedList<crValues>[] coroutines;

        // Start is called before the first frame update
        void Start()
        {
            Timing.transfer t = Timing.getCoroutines();
            coroutine = t.a;
            coroutines = t.bins;
        }

        // Update is called once per frame
        void Update()
        {
            LinkedListNode<crValues> e = coroutine.First;
            while (e != null)
            {
                LinkedListNode<crValues> c = null;
                crValues d = e.Value;
                d.remainingTime -= Time.deltaTime;
                if (d.remainingTime <= 0F)
                {
                    if (d.enumerator.MoveNext())
                    {
                        d.remainingTime = d.enumerator.Current;
                    }
                    else
                    {
                        c = e;
                    }
                    e.Value = d;
                    e = e.Next;
                    if (c != null)
                    {
                        coroutine.Remove(c);
                    }
                }
                else
                {
                    e.Value = d;
                    e = e.Next;
                }
            }
            int i = 0;
            do
            {
                LinkedListNode<crValues> f = coroutines[i].First;
                while (f != null)
                {
                    LinkedListNode<crValues> c = null;
                    crValues d = f.Value;
                    d.remainingTime -= Time.deltaTime;
                    if (d.remainingTime <= 0F)
                    {
                        if (d.enumerator.MoveNext())
                        {
                            d.remainingTime = d.enumerator.Current;
                        }
                        else
                        {
                            c = f;
                        }
                        f.Value = d;
                        f = f.Next;
                        if (c != null)
                        {
                            coroutines[i].Remove(c);
                        }
                    }
                    else
                    {
                        f.Value = d;
                        f = f.Next;
                    }
                }
            }
            while (++i < 5);
        }
    }

    public struct CoroutineHandle
    {
        public LinkedList<CoroutineReplacement1.crValues> bin;
        public LinkedListNode<CoroutineReplacement1.crValues> handle;
    }
}