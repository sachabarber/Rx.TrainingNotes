using System;
using System.Reactive.Linq;
using Moq;
using NUnit.Framework;

namespace DebugginRx
{
    [TestFixture]
    //Discuss the difference between interactively debugging and passively observing - Heisenberg/Observer effect
    public class LoggingWithDo
    {
        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_values_are_produced_THEN_values_are_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                //Add logging in here....
                .Do(x=>logger.Object.Log(x.ToString()))
                .Subscribe();

            //Assert
            logger.Verify(l=>l.Log("0"));
            logger.Verify(l=>l.Log("1"));
            logger.Verify(l=>l.Log("2"));
            logger.Verify(l=>l.Log("3"));
            logger.Verify(l=>l.Log("4"));
        }

        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_sequences_completes_THEN_completion_is_logged()
        {
            var source = Observable.Range(0, 5);
            var logger = new Mock<ILogger>();

            //Act
            source
                .Subscribe(x =>
                {
                    logger.Object.Log(x.ToString());
                },
                ()=>  logger.Object.Log("Completed") 
                );

            //Assert
            logger.Verify(l => l.Log("0"));
            logger.Verify(l => l.Log("1"));
            logger.Verify(l => l.Log("2"));
            logger.Verify(l => l.Log("3"));
            logger.Verify(l => l.Log("4"));
            logger.Verify(l => l.Log("Completed"));
        }

        [Test]
        public void GIVEN_a_logged_observable_sequence_WHEN_sequences_errors_THEN_error_is_logged()
        {
            var source = Observable.Range(0, 5).Concat(Observable.Throw<int>(new Exception("Fail!")));
            var logger = new Mock<ILogger>();

            //Act
            source
                //Add logging in here....
                 .Subscribe(x =>
                {
                    logger.Object.Log(x.ToString());
                },
                ex =>  logger.Object.Log("Fail!") 
                );



            //Assert
            logger.Verify(l => l.Log("0"));
            logger.Verify(l => l.Log("1"));
            logger.Verify(l => l.Log("2"));
            logger.Verify(l => l.Log("3"));
            logger.Verify(l => l.Log("4"));
            logger.Verify(l => l.Log("Fail!"));
        }
    }


}
