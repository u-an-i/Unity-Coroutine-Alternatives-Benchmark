using System.Collections.Generic;
using UnityEngine;

namespace u_i_2
{
    class batch
    {
        private const int size = 4218;

        public struct freed
        {
            public added number;
            public int index;
        }

        public class added
        {
            public added()
            {
                content = new CoroutineReplacement2.crValues[size];
                free = new freed[size];
            }

            public CoroutineReplacement2.crValues[] content;
            public freed[] free;
            public added next = null;
        }

        public batch()
        {
            peakContent = new added();
            baseFreed = peakContent;
            peakFreed = baseFreed;
            first = peakContent;
            iteratorCurrent = first;
        }

        private int iteratorIndex = -1;
        private int peakPeak = 0;
        private int baseFree = 0;
        private int peakFree = -1;
        private static CoroutineReplacement2.crValues nullValue = new CoroutineReplacement2.crValues()
        {
            enumerator = null
        };
        private added peakContent;
        private added baseFreed;
        private added peakFreed;
        private added first;
        private added iteratorCurrent;

        public CoroutineHandle Add(CoroutineReplacement2.crValues value)
        {
            added access;
            int index;
            if(peakFree >= 0)
            {
                freed reference = baseFreed.free[baseFree];
                access = reference.number;
                index = reference.index;
                if (baseFree == peakFree && baseFreed == peakFreed)
                {
                    baseFree = 0;
                    peakFree = -1;
                    baseFreed = first;
                    peakFreed = baseFreed;
                }
                else if(++baseFree == size)
                {
                    baseFree = 0;
                    if (baseFreed == peakContent)
                    {
                        baseFreed = first;
                    }
                    else
                    {
                        baseFreed = baseFreed.next;
                    }
                }
            }
            else
            {
                access = peakContent;
                index = peakPeak;
                if(++peakPeak == size)
                {
                    peakPeak = 0;
                    peakContent.next = new added();
                    peakContent = peakContent.next;
                }
            }
            access.content[index] = value;
            return new CoroutineHandle()
            {
                bin = this,
                access = access,
                index = index
            };
        }

        public ref CoroutineReplacement2.crValues GetNext()
        {
            do
            {
                if (++iteratorIndex == size)
                {
                    iteratorIndex = 0;
                    iteratorCurrent = iteratorCurrent.next;
                }
                if (iteratorCurrent != null && (iteratorCurrent != peakContent || iteratorIndex < peakPeak))
                {
                    ref CoroutineReplacement.crValues value = ref iteratorCurrent.content[iteratorIndex];
                    if (value.enumerator != null)
                    {
                        return ref value;
                    }
                    continue;
                }
                else
                {
                    iteratorIndex = -1;
                    iteratorCurrent = first;
                    return ref nullValue;
                }
            }
            while (true);
        }

        public void DeleteAtIterator()
        {
            iteratorCurrent.content[iteratorIndex].enumerator = null;
            if (++peakFree == size)
            {
                peakFree = 0;
                if (peakFreed == peakContent)
                {
                    peakFreed = first;
                }
                else
                {
                    peakFreed = peakFreed.next;
                }
            }
            peakFreed.free[peakFree] = new freed()
            {
                number = iteratorCurrent,
                index = iteratorIndex
            };
        }

        public void DeleteWith(added access, int index)
        {
            access.content[index].enumerator = null;
            if (++peakFree == size)
            {
                peakFree = 0;
                if (peakFreed == peakContent)
                {
                    peakFreed = first;
                }
                else
                {
                    peakFreed = peakFreed.next;
                }
            }
            peakFreed.free[peakFree] = new freed()
            {
                number = access,
                index = index
            };
        }

        public void DeleteAll()
        {
            peakContent = new added();
            baseFreed = peakContent;
            peakFreed = baseFreed;
            first = peakContent;
            iteratorCurrent = first;
            iteratorIndex = -1;
            peakPeak = 0;
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
            return a.Add(new CoroutineReplacement2.crValues()
            {
                remainingTime = 0F,
                enumerator = coroutine
            });
        }

        public static CoroutineHandle RunCoroutine(IEnumerator<float> coroutine, int bin)
        {
            return bins[bin].Add(new CoroutineReplacement2.crValues()
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
            handle.bin.DeleteWith(handle.access, handle.index);
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

    class CoroutineReplacement2 : MonoBehaviour
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
            ref crValues value = ref a.GetNext();
            while (value.enumerator != null)
            {
                value.remainingTime -= Time.deltaTime;
                if (value.remainingTime <= 0F)
                {
                    if (value.enumerator.MoveNext())
                    {
                        value.remainingTime = value.enumerator.Current;
                    }
                    else
                    {
                        a.DeleteAtIterator();
                    }
                }
                value = ref a.GetNext();
            }
            int i = 0;
            do
            {
                batch b = bins[i];
                ref crValues val = ref b.GetNext();
                while (val.enumerator != null)
                {
                    val.remainingTime -= Time.deltaTime;
                    if (val.remainingTime <= 0F)
                    {
                        if (val.enumerator.MoveNext())
                        {
                            val.remainingTime = val.enumerator.Current;
                        }
                        else
                        {
                            b.DeleteAtIterator();
                        }
                    }
                    val = ref b.GetNext();
                }
            }
            while (++i < 5);
        }
    }

    struct CoroutineHandle
    {
        public batch bin;
        public batch.added access;
        public int index;
    }
}