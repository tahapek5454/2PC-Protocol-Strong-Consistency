# 2PC-Protocol-Strong-Consistency
The aim of this project is to process the Strong Consistency  by following the best practices with  Two-Phase Commit(2PC) Protocol  on NETCore. 

### Getting Started
+ The project is created with .NetCore 7.
+ Minimal APIs are utilized within the project.
+ PostgreSQL is used as the database.
  
### What is Strong Consistency ?
It is a behavior aimed at guaranteeing the always-up-to-date state of data.
+ Short-term inconsistency is not an issue.
+ Preferred in financial transactions.
+ May impact performance and scalability at times because the coordination of data updates is required.
+ Locks all services for consistency, leading to high costs and partial performance decreases.

### Strong Consistency - Two Phase Commit (2PC) Protocol
+ 2PC is a protocol used in distributed systems to ensure data consistency with a strong consistency model.
+ Used to manage coordination between multiple services and handle data inconsistency.
+ Its primary purpose is to guarantee whether a transaction is completed atomically on all resources.
+ 2PC consists of two phases: Prepare Phase and Commit Phase.
+ **Prepare Phase**
    + Coordinator receives the request from the user.
    + After receiving the request, the coordinator sends a message to all nodes involved in the operation, asking if they are ready, and expects responses from all participants regarding this message.
    + If participants receive an Ok message, the coordinator initiates the second phase.
    + If at least one participant receives a message like No or no response is received, the request is canceled without proceeding to the second phase.
+ **Commit Phase**
    + Coordinator, after ensuring that all participants are ready, sends a commit message to initiate the operations for all services due to their responsibilities.
    + Coordinator awaits responses such as 'Ack' from participants after they complete their operations.
    + If responses from all participants are received, the request is successfully completed.
    + If a response is not received from at least one participant, the transaction is canceled, and an abort message is sent to undo their actions from all services.
+ **Concepts**
    + Coordinator: A service in a central administrative position that controls all steps.
    + Participants: Nodes, Services, Microservices. Entities executing the operations.

