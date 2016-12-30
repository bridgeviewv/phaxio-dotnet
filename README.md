﻿# Phaxio

![AppVeyor build status](https://ci.appveyor.com/api/projects/status/s9gjrnfa42g08mb4?svg=true)

[![Build Status](https://travis-ci.org/phaxio/phaxio-dotnet.svg?branch=master)](https://travis-ci.org/phaxio/phaxio-dotnet)


Phaxio is the only cloud based fax API designed for developers. This is the .NET client library for Phaxio.

## Getting started

First, [sign up](https://console.phaxio.com/signup) if you haven't already.

Second, go to [api settings](https://console.phaxio.com/apiSettings) and get your key and your secret.

Third, install this client with Nuget:

    Install-Package Phaxio

For basic usage, use [this guide](Docs/README-csharp-v2.md) if you're a C# developer or
[this guide](Docs/README-vb-v2.md) if you're a VB.NET developer.

If you're looking for version 1, use [this guide](Docs/README-csharp.md) if you're a C# developer or
[this guide](Docs/README-vb.md) if you're a VB.NET developer.

## Migration from library version 1.0.0 to 2.0.0

PhaxioClient no longer has a constructor that accepts RestSharp.RestClient.

## Migration from API V1 to V2

Phaxio is not a drop-in replacement for PhaxioClient. Several methods have been removed.

- AttachPhaxCodeToPdf was removed
- CreatePhaxCode is now GeneratePhaxCode
- DownloadPhaxCodePng is now DownloadPhaxCode
- GetHostedDocument was removed

Faxes are now sent by creating a FaxRequest object and then passing it to PhaxioV2Client#SendFax. The fields
of FaxOptions have been moved to FaxRequest except for the following:

- StringData and StringDataType are no longer supported
- Batch is no longer necessary

## Errors

Sometimes errors will occur, whether it's network timeouts or bad requests. All operations will throw an
exception if a network or client error occurs. Some operations return data to you, such as a byte array
representing a PDF. If those requests fail due to an authentication failure, a malformed number, insufficient
funds, rate limiting, etc., they'll throw an ApplicationException with an error message describing the problem.

For those requests returning a Result object, that will have a bool named Success and a string called
Message that will tell you the result of the operation.

### Rate limiting

The Phaxio API is rate limited. If you make too many requests too quickly, you might receive this error.
Check the exception message, wait a second, and then try your request again.

## Writing callbacks (webhooks)

Writing a callback to get fax send or recieve events is simple. Read this [handy guide](Docs/README-callbacks.md) to get started.

&copy; 2016 Phaxio
