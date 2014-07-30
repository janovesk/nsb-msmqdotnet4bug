nsb-msmqdotnet4bug
==================
From .NET 3.5 to .NET 4.0 Microsoft changed made almost every method on MessageQueue thread UNSAFE. This makes the implementation of the MsmqWorkerAvailabilityManager not safe. 

Steps to repro:

Generate a "worker available" message
1) Sett WorkerThreads to 0 in app.config
2) Build solution.
3) Run it with NServiceBus.Host.exe NServiceBus.Integration NServiceBus.Master
4) Copy the generated "worker available" message on the queue "test.distributor.control" to a safe queue. Create 100 copies of the message.

Run the repro
5) Set WorkerThreads back to 10 in app.config.
6) Rebuild
7) Run it with NServiceBus.Host.exe NServiceBus.Integration NServiceBus.Distributor
8) Copy the 100 messages created in 4 to "test.distributor.control"
9) Expected result should be 0 messages in "test.distributor.control" and 0 messages in "test.error".  Actual result is that many of the messages end up on "test.error" with the following error message:

2014-07-30 18:50:52,843 [Worker.16] WARN  NServiceBus.Unicast.Transport.Transactional.TransactionalTransport [(null)] <(null)> - Failed raising 'transport messa
ge received' event for message with ID=85eb7281-2fd2-40a1-a197-885dd22db0b9\4971606
System.InvalidOperationException: Property ResponseQueue was not retrieved when receiving the message. Ensure that the PropertyFilter is set correctly.
   at System.Messaging.Message.get_ResponseQueue()
   at NServiceBus.Distributor.MsmqWorkerAvailabilityManager.MsmqWorkerAvailabilityManager.ClearAvailabilityForWorker(Address address) in c:\BuildAgent\work\d4de
8921a0aabf04\src\distributor\NServiceBus.Distributor.MsmqWorkerAvailabilityManager\MsmqWorkerAvailabilityManager.cs:line 35
   at NServiceBus.Distributor.DistributorReadyMessageProcessor.HandleControlMessage(TransportMessage controlMessage) in c:\BuildAgent\work\d4de8921a0aabf04\src\
distributor\NServiceBus.Distributor\DistributorReadyMessageProcessor.cs:line 59
   at NServiceBus.Unicast.Transport.Transactional.TransactionalTransport.OnTransportMessageReceived(TransportMessage msg) in c:\BuildAgent\work\d4de8921a0aabf04
\src\impl\unicast\transport\NServiceBus.Unicast.Transport.Transactional\TransactionalTransport.cs:line 496

