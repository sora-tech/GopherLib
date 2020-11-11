# GopherLib
 A TDD Gopher Library written in C#.

## Why Gopher?
This is my third gopher library which may come as a surprise as I am not quite old enough to have used Gopher originally.  Gopher is an alternative protocol to the HTTP/HyperText family that uses a menu oriented document based approach.

The protocol is very simple especially in comparison to HTTP and lacks any of the security or speed concerns of HTTPS and later HTTP revisions.  The simplicity allows for browsing with a raw TCP connection which lead to early success on the low-bandwidth early internet.

## Specification

ITEF have published several RFCs covering Gopher which provide the baseline specification.  There are further additions covered by the Gopher+ extensions although these are not as well documented.

* [RFC 1436 - The Internet Gopher Protocol](https://tools.ietf.org/html/rfc1436)
* [RFC 1738 - Uniform Resource Locators](https://tools.ietf.org/html/rfc1738)
* [RFC 4266 - The Gopher URI Scheme](https://tools.ietf.org/html/rfc4266)
* [Gopher+ Wikipedia Article](https://en.wikipedia.org/wiki/Gopher%2B)

Gopher support has been dropped by all major browsers so for testing [Gopher Browser for Windows](http://www.jaruzel.com/gopher/gopher-client-browser-for-windows) is used.

[SDF.org](https://sdf.org) provide free Gopher hosting and are used as a remote server for validation.

## Goals

This library is intended to support both client and server applications.  A client is quite straightforward as little more than opening a TCP connection and sending plain text is required.  Testing around network interactions is historically difficult so this should provide some challenge for a TDD based approach.


## Challenges

### Span \<T>
`Span<T>` and in my case its use as `Span<byte>` is a "new" feature of .NET introduced at the start of 2018 in .NET Core 2 and back-ported via `System.Memory` to Framework.  The goal of this new structure is:
>  "[...] they are designed so that some or all of the data can be efficiently passed to components in the pipeline, which can process them and optionally modify the buffer.

With this in mind I saw the `TcpClient` output as a good candidate to use these and see how behave.  Replacing `byte[]` with `Span<byte>` should have been a simple job as I was not doing any significant manipulation of the result data currently.  Updating the signatures means nothing compiles so code must be changed before tests can be re-run.  Expecting this to be straightforwards I replaced `return byte[]` with `return new Span<byte>(byte[])` and was pleasantly surprised to find that was it for my code.

Updating the tests involved two parts: the asserts to deal with this new type and; the mocking to return this type where required.  I'm using NUnit for my testing which normally results in a `Assert.AreEqual(T1, T2)` statement.  When attempting to compare `Span<byte>` this produced an error: 
> CS1503 Argument 1: cannot convert from 'System.Span<byte>' to 'object'

Looking over the NUnit documentation has no mention of Span despite the last update to NUnit being in 2019.  While I was not aware that this was the symptom of a bigger issue I resolved it by calling `span.ToArray()` allowing the resulting `byte[]` to be compared.

Next was updating the NSubstitute mocks to deliver `Span<byte>` when required.  Changing the return object is obviously straightforwards but a new error appeared:
> CS0306 The type 'Span<byte>' may not be used as a type argument

This error is due to the special implementation of `Span<T>` only existing on the stack. Looking through the documentation and searching for usages of `Span<T>` with NSubstitute turned up nothing despite the latest release being sometime in 2019.  There were some results about Moq possibly supporting it so I decided to try that but unfortunately ended up with a similar problem:
> CS8640 Expression tree cannot contain value of ref struct or restricted type 'Span'

Finally I realized that these frameworks are to help create mock objects but are not the only way.  Creating a normal C# object that implements an interface is the simplest way to solve this problem and has no limitations on what it returns.  I created several small concrete mocks which could return known data as required and this allowed testing to be done.
