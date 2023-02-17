# AWSFundamentals

### Description
An AWS fundamentals course (https://nickchapsas.com/courses/enrolled/1993904)

### Dependencies:
* AWSSDK.SQS

### Create a publisher and consumer of a SQS Queue

* Install AWS cli by going to https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html and download the msi for windows.
* Go to security credentials under username.
    * Scroll down to access tokens and generate a root access token. 
* Run "AWS configure"
    * Set the access token.
    * Set the secret token.
    * Set the region for your account.
    * Set the default format to "json".
### Projects

##### Sqs

* Search for sqs (Simple Queue Service) and click create queue.
    * Type: Standard
    * Name: customers
* Create dead letter queue.
    * Type: Standard
    * Name: customers-dlq
    * Redrive allow policy: Enabled
        * Redrive permission: By queue
        * ARN for each source queue: customers
* Go back to customers queue.
    * Dead-letter queue: Enabled
        * Queue: customers-dlq
        * Maximum receives: 3
* Go to customers-dlq and click start DLQ redrive which will push all dead letters currently in queue back to customers queue.

##### SqsPublisher and SqsConsumer

* SqsPublisher puts a message into the customers queue.
* SqsConsumer reads that message from the queue and outputs it to the console window and after that deletes it after otherwise it will be put back into the queue.

##### Customers.Api and Customers.Consumer

* Customers.Api provides the capability of adding, updating and deleting records from a database and adding messages to the customers queue after completion of those operations.
* Customers.Consumer runs a background service which monitors the customers queue and reads those messages and outputs those messages to the console window. If a message failurs to be processed 3 consecutive times, that message will be put in the dead letter queue (customers-dlq). To mimic this, throw an exception in one of the Handlers handle method.