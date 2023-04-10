# AWSFundamentals

### Description
An AWS fundamentals course (https://nickchapsas.com/courses/enrolled/1993904)

### Dependencies:
* AWSSDK.SQS
* AWSSDK.SimpleNotificationService
* AWSSDK.DynamoDBv2

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

* Messages are sent to a sqs where a consumer can read those messages.
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

##### Sns

* A message is published to an sns which then pushes those messages to subscriptions (sqs) and those subscriptions can decide what messages should be pushed.
* Search for sns (Simple Notification Service) and click create topic with name customers.
* Go to customers topic and create subscription.
    * Topic: ....customers
    * Protocol: Amazon SQS
    * Endpoint: ....customers
* To publish message to customers queue from topic you will need to update customers queue first.
    * Go to customers queue -> Access Policy and add the following:
        * {
      "Effect": "Allow",
      "Principal": {
        "Service": "sns.amazonaws.com"
      },
      "Action": "sqs:SendMessage",
      "Resource": "arn:aws:sqs:us-east-1:610403247034:customers2",
      "Condition": {
        "ArnEquals": {
          "aws:SourceArn": "arn:aws:sns:us-east-1:610403247034:customers"
        }
      }
    }
* To add another queue named customers2 follow the above 2 points.
* Subscription filters can be added by clicking on a subscription and clicking edit and scrolling down to subscription filter policy. An Example to add for message body would be:
    * {
  "Fullname": [
    "Jarryd Deane"
  ]
}
* Note that the Customers.Consumer project from sqs can be used along with the Customers.Api from sns.

### DynamoDb

##### Customers.Api

* Customers are stored in a DynamoDb.
* A user is able to create, update, delete and retrieve a single or all customers.
* See CustomerRepository for implementation.
* The "customers" database has been manually created in AWS with the "pk" and "sk" being the unique keys and being named as such.
    * A global index has also been created with partition key being "email" and sort key being "Id".
        * You can create it by going to the table and clicking on the index tab and clicking create index.

##### Movies.Api

* Movies are seeded to two databases namely:
    * movies-year-title
    * movies-title-wrotten
* The above tables are manually created in AWS with "pk" and "sk" being the unique keys and being named as such.
* In the Program.cs file, a transaction is created to insert two records into two separate tables.
* A "movies" database has been manually created in AWS with customized tables settings turned on and a secondary index added named "RottenTomatoesPercentage" (sort key name) and "rotten-index" (index name).
    * You can search using the exact code we do for the global index in customers.
    * Dynamo Db streams is a feature that allows one to be notified when a change has been made in the database such as adding and updating and get the previous and/or new values or just the pk/sk. You can use this instead of Sns/Sqs.

### S3

* Search for s3 and create a bucket called "jarrydawscourse".
* You can add a file to that bucket by clicking on the bucket and there should be a upload button to click.
    * You can upload a single file or a folder with multiple files.
    * You can share a link to the uploaded file temporarily by clicking actions -> share with a presigned url.
