#Overview of Asynchrony

The space of asynchronous programming can be a complex, confusing and sometimes counter-intuitive domain to work in.
To acquire the skills to work with Rx in a productive manner, one needs more than just an appreciation for asynchronous coding, but a fairly strong understanding of it.
In this workshop we will ensure that you have a solid foundation of asynchronous coding skills on which to build your Rx skill set.


##Synchronous vs Asynchonous vs Concurrent
Maybe a non-technical story is the quickest way to describe synchronous, asynchronous and concurrent programming. 

Suppose you are running errands for the morning.
You have to pick up a delivery from the post office, take your child to get a haircut and get you car serviced.
All these things can be done in the local village and each place is next to each other.


###The Synchronous Example
You drive to the post office and request your post. 
You wait with your child while the postman looks out back for your package. 
In 5 minutes the postman returns with your package.
You go next door and drop your child off to get a hair cut.
You sit with them while you wait to be served and then wait while the hair cut is done.
Once you have paid for the hair cut you drive the car across the road and drop it off for a service. 
You wait for the service with your child.
The service is completed, you pay and you go home with your child.

That is an example of a synchronous program.

  
###The Asynchronous Example
You drop your car off to be serviced.
You go to the post office and show your missed delivery note.
The postman say he will be 5 minutes.
You take you child to get a haircut.
You sit with the child while you wait.
Once the child is being served you go back to the post office to pick up your package.
The postman has it ready for you, you swap your package for the delivery note.
You wait outside the Hairdresser and Garage.
You see the child is nearly finished, you go inside pick them up and pay.
You then go across the road to wait for the car to be finished.
The service is completed, you pay and you go home with your child.  

That is an example of a single threaded asynchronous program.

###The Concurrent Example
You drive your child and two friends to the village.
You drop one friend off at the hairdresser with the child.
You drop the other off at the post office.
You go to the garage to get the car serviced.
Once the car is serviced, you pick up your two friends and child and go home. 
That is an example of a multi-threaded/concurrent program.








##Synchronous programming
First let us start off with what is synchronous.
A synchronous program is one that will execute one statement after another, in order.
One statement can not begin until the previous one has completed.

Synchronous programming tends to be very easy write, reason about and debug.

Consider a program where we need to read from the disk, read from the network and compute a value based on the result of these two actions.
A synchronous program would need to perform the read


###Asynchronous programming

###Concurrent programming

Simply doing two things at once, concurrently. 
Concurrent programming (assuming a join at some point) is a form of asynchronous programming.



* multi threaded, process or server is the same distributed programming problem space. On investigation the problem is fractal. The CPU sockets have multiple core with various levels of caches that share the same problems and solutions as a Google map-reduce cluster.

##Legacy patterns
 * APM
 * WaitHandles (Auto & Manual ResetEvents, Mutex, Semaphore), Monitors
 * Threads (Foreground vs Background)

##Futures & promises
Language agnostic (try to find Scala and JS versions).
 * In general a Future is the Async value, and a Promise is the Async function to provide that value. (In some implementations of Futures multiple promises can amb to produce a result for the future)
 * Futures in .NET as Task<T>
 * Long running computations with Tasks
 * I/O with Tasks
 * Continuations, Error handling and cancellation

##Language support (Async/Await)


* Review legacy .NET patterns
 * Futures in .NET as Task<T>
 * Long running computations with Tasks
 * I/O with Tasks
 * Continuations, Error handling and cancellation
 * Async/await
 



Further reading :
http://cs.brown.edu/courses/cs168/f12/handouts/async.pdf