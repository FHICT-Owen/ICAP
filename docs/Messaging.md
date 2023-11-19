# Messaging in ICAP
ICAP is distributed application that makes use of the microservices architecture. The microservices within ICAP have to be able to communicate with each other using a certain messaging system. The platform used to facilitate this messaging will be Azure Service Bus. This document will show the events that will be consumed and produced by the various microservices of ICAP. The general message structure that will be re-used across all microservices will also be described here. 

## Message Structure
All of the messages within ICAP will be JSON objects. 

## Message Types