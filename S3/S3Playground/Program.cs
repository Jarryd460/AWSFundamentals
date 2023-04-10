using Amazon.S3;
using Amazon.S3.Model;

var s3Client = new AmazonS3Client();

await using var mapInputStream = new FileStream("./Map.png", FileMode.Open, FileAccess.Read);

var mapPutObjectRequest = new PutObjectRequest()
{
    BucketName = "jarrydawscource",
    Key = "images/Map.png",
    ContentType = "image/png",
    InputStream = mapInputStream
};

await s3Client.PutObjectAsync(mapPutObjectRequest).ConfigureAwait(false);

await using var moviesInputStream = new FileStream("./movies.csv", FileMode.Open, FileAccess.Read);

var moviesPutObjectRequest = new PutObjectRequest()
{
    BucketName = "jarrydawscource",
    Key = "files/movies.csv",
    ContentType = "text/csv",
    InputStream = moviesInputStream
};

await s3Client.PutObjectAsync(moviesPutObjectRequest).ConfigureAwait(false);