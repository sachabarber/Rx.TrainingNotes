using System;
using HotAndCold._03_LazySamples;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace HotAndCold
{
    //Without changing these tests, correct the implementation in MyModel.GetData()
    [TestFixture]
    public class LazySequences : ReactiveTest
    {
        [Test]
        public void GetData_only_connects_when_subscribed_to()
        {
            //Arrange
            var myServiceMock = new Mock<IMyService>();
            var myModel = new MyModel(myServiceMock.Object);

            //Act
            myModel.GetData();  //Note the absence of a subscription.

            //Assert
            myServiceMock.Verify(svc=>svc.Connect(), Times.Never);
        }

        [Test]
        public void GetData_connects_when_subscribed_to()
        {
            //Arrange
            var observer = new TestScheduler().CreateObserver<string>();
            var myServiceMock = new Mock<IMyService>();
            var myModel = new MyModel(myServiceMock.Object);

            //Act
            myModel.GetData().Subscribe(observer);

            //Assert
            myServiceMock.Verify(svc => svc.Connect(), Times.Once);
        }
        
        [Test]
        public void GetData_surfaces_myService_DataReceived_payload()
        {
            //Arrange
            var observer = new TestScheduler().CreateObserver<string>();
            var myServiceMock = new Mock<IMyService>();
            var myModel = new MyModel(myServiceMock.Object);

            //Act
            myModel.GetData().Subscribe(observer);
            myServiceMock.Raise(svc=>svc.DataReceived+=null, new DataReceivedEventArgs("expected"));

            //Assert
            observer.Messages.AssertEqual(
                OnNext(0, "expected"));
        }
        
        [Test]
        public void GetData_disposes_connection_when_subscription_is_disposed()
        {
            //Arrange
            var observer = new TestScheduler().CreateObserver<string>();
            var myServiceMock = new Mock<IMyService>();
            var myConnection = new Mock<IDisposable>();
            myServiceMock.Setup(svc => svc.Connect()).Returns(myConnection.Object);
            var myModel = new MyModel(myServiceMock.Object);

            //Act
            var subscription = myModel.GetData().Subscribe(observer);
            subscription.Dispose();

            //Assert
            myConnection.Verify(conn => conn.Dispose(), Times.Once);
        }
    }
}