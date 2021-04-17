using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace NumberGenerator
{
    public class Generator
    {
        private bool _running;
        
        public IObservable<long> Generate(long max, long intervalInMilliseconds)
        {
            IntervalInMilliseconds = intervalInMilliseconds;
            _running = true;
            var subject = new Subject<long>();
            Start(subject, max);
            return subject;
        }

        public long IntervalInMilliseconds { set; get; }

        public void Stop()
        {
            _running = false;
        }

        private void Start(IObserver<long> subject, long max)
        {
            Task.Run(async () =>
            {
                var rand = new Random();
                while (_running)
                {
                    subject.OnNext(LongRandom(2, max, rand));
                    await Task.Delay(TimeSpan.FromMilliseconds(IntervalInMilliseconds));
                }
                
                subject.OnCompleted();
            });
        }
        
        private static long LongRandom(long min, long max, Random rand) {
            var buf = new byte[8];
            rand.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand % (max - min)) + min;
        }
    }
}