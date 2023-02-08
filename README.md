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