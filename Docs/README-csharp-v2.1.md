# Usage examples - C&#35;

## Basics

The PhaxioClient class is the entry point for any Phaxio operation.

    var phaxio = new Phaxio(key, secret);
    
### Getting your account status

    var account = phaxio.Account;
    Console.WriteLine(string.Format("Balance: {0}", account.Balance));

## Errors

Operations that connect to Phaxio will throw an exception if an error is encountered.

RateLimitException happens if you have made too many requests per second.
InvalidRequestException is throw if the data sent to Phaxio is not correct
AuthenticationException gets thrown when your credentials are invalid
NotFound is throw when you try to retrieve a resource by ID but it isn't found
APIConnectionException occurs when there is a network issue
GeneralException happens if the server is not working

## Faxes

### Sending a fax

At the heart of the Phaxio API is the ability to send a fax:

    var fax = phaxio.Faxes.Create(toNumber: "8088675309", file: "form1234.pdf");

The fax object can be used to reference your fax later. Well, now, wasn't that simple? You can also pass in a Dictionary<string, object> to create your fax:

    var faxConfig = new Dictionary<string, object>
    {
        { "ToNumber", "8088675309" },
        { "File", "form1234.pdf" }
    };
    var fax = phaxio.Faxes.Create(faxConfig);

For a list of options, see below:
    
If you have more than one file, you can pass in a list and Phaxio will concatenate them into one fax:

    var pdf1 = new FileInfo("form1234.pdf");
    var pdf2 = new FileInfo("form4321.pdf");
    var faxConfig = new Dictionary<string, object>
    {
        { "ToNumber", "8088675309" },
        { "Files", new List<FileInfo> { pdf1, pdf2 } },
        { "CallerId", "2125552368" }
    };
    var fax = phaxio.Faxes.Create(faxConfig);

If you have a bunch of faxes going to one number, you might want to check out [batching](https://www.phaxio.com/docs/api/send/batching/).
You first specify a batch delay in the FaxOptions. Then, you send as many faxes as you'd like to the number in question, 
and when you're finished and the batch delay is expired, Phaxio will send them all as one long fax. Here's what a
batching FaxOptions would look like:
    
    var fax1Config = new Dictionary<string, object>
    {
        ToNumber = "8088675309",
        File = pdf1,
        BatchDelaySeconds = 30
    };
    var fax2Config = new Dictionary<string, object>
    {
        ToNumber = "8088675309",
        File = pdf2,
        BatchDelaySeconds = 30
    };
    phaxio.Faxes.Create(fax1Config);
    phaxio.Faxes.Create(fax2Config);

The machine at 808-867-5309 will see pdf1 and pdf2 as one long fax.

### Querying sent faxes

To see your sent faxes after you've sent it, call Retrieve on the Faxes property:

    var fax = phaxio.Faxes.Retrieve("1234");
    
This returns a single Fax or throws a NotFound exception. You can also add filters:

    var faxes = phaxio.Faxes.List(createdBefore: DateTime.Now);

This returns an IEnumerable<Fax>.

### Downloading a fax

To retrieve the fax file after you've sent it, call the File property on the Fax instance:

    var file = phaxio.Faxes.Retrieve("1234").File;
    
File is a byte array representing your fax in PDF form that you can write to disk or store in a database.
You can also specify which format you'd like using the File method on the Fax instance:

    var file = phaxio.Faxes.Retrieve("1234").File(FileFormats.SMALL_JPEG);
    
Specify FileFormats.SMALL_JPEG for a small JPEG, FileFormats.LARGE_JPEG for a large JPEG, or FileFormats.PDF for PDF. If you don't specify this, it will be a PDF.

### Resending a fax

You can resend a fax:

    fax.Resend();

### Cancelling a fax

You can cancel a fax:

    fax.Cancel("1234");

### Deleting a fax's files

You can delete a fax:

    fax.DeleteFiles();

### Deleting a fax

You can delete a fax:

    fax.Delete();

## Numbers

### Getting area codes 

If you want to know what area codes are available for purchase, you can call this property:

    IEnumberable<AreaCode> areaCodes = phaxio.AreaCodes;
    
This returns a IEnumberable of AreaCode objects. You can also optionally request tollfree numbers:

    IEnumberable<AreaCode> areaCodes = phaxio.AreaCodes(tollFree:true);

You can specify the state (and you must specify either the country code or the country if you do):

    IEnumberable<AreaCode> areaCodes = phaxio.AreaCodes.List(state:"MA", country:"US");

Or both:

    IEnumberable<AreaCode> areaCodes = phaxio.AreaCodes.List(tollFree:true, state:"MA", country:"US");

### Provisioning a number

You can ask Phaxio to get you a new number (you must specify an area code and country code):

    var newNumber = phaxio.FaxPhoneNumbers.Provision("808", "1");

The call returns a FaxPhoneNumber object representing your new number.

You can also specify a callback URL that will be called when a fax is recieved
at the new number (this will override the default callback URL).

    var newNumber = phaxio.FaxPhoneNumbers.Provision("808", "https://example.com/callback");
    
### Listing your numbers

To get a list of your numbers, you can run this method:

    var numbers = phaxio.FaxPhoneNumbers;

which will return a IEnumberable<FaxPhoneNumber> with all of your numbers.

You can specify an country code to search in:

    var numbers = phaxio.FaxPhoneNumbers.List(countryCode: "1");

You can specify an area code to search in (you must also specify the country code):

    var numbers = phaxio.FaxPhoneNumbers.List(areaCode: "808", countryCode: "1");

or you can retrieve a specific number:
    
    var number = phaxio.FaxPhoneNumbers.Retrieve("8088675309");
    
### Release number

You can a release number (give it back to Phaxio):

    number.Release();

This operation will throw an exception if the number cannot be released.
    
## PhaxCodes

## Creating a PhaxCode

Creating a PhaxCode is simple:

    var phaxCode = phaxio.PhaxCodes.Create();

You can also attach metadata to the code so you can reference it later:

    var phaxCode = phaxio.PhaxCodes.Create("code-for-form1234");

You can also get the image directly:

    File.WriteAllBytes(@"C:\temp\phaxCode.png", phaxCode.Png);

To get the properties of the newly generated code:

    var phaxCode = phaxio.PhaxCodes.Retrieve(codeId);

To download the PNG of this newly generated code:

    var codeBytes = phaxio.PhaxCodes.Retrieve(codeId).Png;

## Misc

### Getting supported countries

If you want to know what countries are supported by Phaxio, you can call this method:

    IEnumerable<Country> supportedCountries = phaxio.SupportedCountries;
    
This returns a IEnumerable of Country objects that have pricing and the services available.
 
### Testing callbacks (web hooks)

So you've written a callback or a webhook you'd like tested. It's simple to have Phaxio send you a fax:

    var testConfig = new Dictionary<string, object>
    {
        File = new FileInfo("test-fax.pdf")
    };
    var fax = phaxio.Faxes.TestRecieveCallback(testConfig);
    
This returns a Fax object. This will call your default account callback. If you've specified a callback
for an individual number and you'd like to test that callback, you can specify it with the toNumber parameter:

    var testConfig = new Dictionary<string, object>
    {
        ToNumber = "8088675309",
        File = new FileInfo("test-fax.pdf")
    };
    var fax = phaxio.Faxes.TestRecieveCallback(testConfig);

You can also fake who the fax is from:

    var testConfig = new Dictionary<string, object>
    {
        FromNumber = "2125552368",
        File = new FileInfo("test-fax.pdf")
    };
    var fax = phaxio.Faxes.TestRecieveCallback(testConfig);

&copy; 2016-2017 Phaxio