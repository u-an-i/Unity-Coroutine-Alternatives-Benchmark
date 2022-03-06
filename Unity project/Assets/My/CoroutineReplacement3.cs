using System.Collections.Generic;
using UnityEngine;

namespace u_i_3
{
    class batch
    {
        private const int size = 4218;

        public batch()
        {
            content = new List<CoroutineReplacement3.crValues>(size);
            freed = new List<int>(size);
            freed.Add(0);
        }

        private int iteratorIndex = -1;
        private int baseFree = 0;
        private int peakFree = -1;
        private static CoroutineReplacement3.crValues nullValue = new CoroutineReplacement3.crValues()
        {
            enumerator = null
        };
        private List<CoroutineReplacement3.crValues> content;
        private List<int> freed;

        public CoroutineHandle Add(CoroutineReplacement3.crValues value)
        {
            int index;
            if(peakFree >= 0)
            {
                index = freed[baseFree];
                content[index] = value;
                if (baseFree == peakFree)
                {
                    baseFree = 0;
                    peakFree = -1;
                }
                else if(++baseFree == freed.Count)
                {
                    baseFree = 0;
                }
            }
            else
            {
                index = content.Count;
                content.Add(value);
            }
            return new CoroutineHandle()
            {
                bin = this,
                index = index
            };
        }

        public CoroutineReplacement3.crValues GetNext()
        {
            if(++iteratorIndex < content.Count)
            {
                CoroutineReplacement3.crValues value = content[iteratorIndex];
                return (value.enumerator != null ? value : GetNext());
            }
            else
            {
                iteratorIndex = -1;
                return nullValue;
            }
        }

        public void ReplaceAtIterator(CoroutineReplacement3.crValues value)
        {
            content[iteratorIndex] = value;
        }

        public void DeleteAtIterator()
        {
            CoroutineReplacement3.crValues value = content[iteratorIndex];
            value.enumerator = null;
            content[iteratorIndex] = value;
            if ((++peakFree - baseFree) % freed.Count == 0 && peakFree > 0)
            {
                baseFree = 0;
                peakFree = freed.Count;
                freed.Add(iteratorIndex);
            }
            else
            {
                if(peakFree != freed.Count)
                {
                    freed[peakFree] = iteratorIndex;
                }
                else
                {
                    peakFree = 0;
                    freed[peakFree] = iteratorIndex;
                }
            }
        }

        public void DeleteWith(int index)
        {
            CoroutineReplacement3.crValues value = content[index];
            value.enumerator = null;
            content[index] = value;
            if ((++peakFree - baseFree) % freed.Count == 0 && peakFree > 0)
            {
                baseFree = 0;
                peakFree = freed.Count;
                freed.Add(index);
            }
            else
            {
                if (peakFree != freed.Count)
                {
                    freed[peakFree] = index;
                }
                else
                {
                    peakFree = 0;
                    freed[peakFree] = index;
                }
            }
        }

        public void DeleteAll()
        {
            content = new List<CoroutineReplacement3.crValues>(size);
            freed = new List<int>(size);
            freed.Add(0);
            iteratorIndex = -1;
            baseFree = 0;
            peakFree = -1;
        }
    };

    static class Timing
    {
        private static batch a = new batch();
        private static batch[] bins = new batch[5]
        {
            new batch(),
            new batch(),
            new batch(),
            new batch(),
            new batch()
        };

        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine)
        {
            return a.Add(new CoroutineReplacement3.crValues()
            {
                remainingTime = 0F,
                enumerator = coroutine
            });
        }

        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, int bin)
        {
            return bins[bin].Add(new CoroutineReplacement3.crValues()
            {
                remainingTime = 0F,
                enumerator = coroutine
            });
        }

        public static float WaitForSeconds(float duration)
        {
            return duration;
        }

        public static void KillCoroutines(CoroutineHandle handle)
        {
            handle.bin.DeleteWith(handle.index);
        }

        public static void KillCoroutines(int bin)
        {
            bins[bin].DeleteAll();
        }

        public static transfer getCoroutines()
        {
            return new transfer()
            {
                a = a,
                bins = bins
            };
        }

        public struct transfer
        {
            public batch a;
            public batch[] bins;
        };
    }

    class CoroutineReplacement3 : MonoBehaviour
    {
        public struct crValues
        {
            public float remainingTime;
            public IEnumerator<float> enumerator;
        };

        private batch a;
        private batch[] bins;

        // Start is called before the first frame update
        void Start()
        {
            Timing.transfer coroutines = Timing.getCoroutines();
            a = coroutines.a;
            bins = coroutines.bins;
        }

        // Update is called once per frame
        void Update()
        {
            crValues value = a.GetNext();
            while (value.enumerator != null)
            {
                value.remainingTime -= Time.deltaTime;
                if (value.remainingTime <= 0F)
                {
                    if (value.enumerator.MoveNext())
                    {
                        value.remainingTime = value.enumerator.Current;
                        a.ReplaceAtIterator(value);
                    }
                    else
                    {
                        a.DeleteAtIterator();
                    }
                }
                else
                {
                    a.ReplaceAtIterator(value);
                }
                value = a.GetNext();
            }
            int i = 0;
            do
            {
                batch b = bins[i];
                crValues val = b.GetNext();
                while (val.enumerator != null)
                {
                    val.remainingTime -= Time.deltaTime;
                    if (val.remainingTime <= 0F)
                    {
                        if (val.enumerator.MoveNext())
                        {
                            val.remainingTime = val.enumerator.Current;
                            b.ReplaceAtIterator(val);
                        }
                        else
                        {
                            b.DeleteAtIterator();
                        }
                    }
                    else
                    {
                        b.ReplaceAtIterator(val);
                    }
                    val = b.GetNext();
                }
            }
            while (++i < 5);
        }
    }

    struct CoroutineHandle
    {
        public batch bin;
        public int index;
    }
}