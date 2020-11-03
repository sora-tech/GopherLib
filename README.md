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