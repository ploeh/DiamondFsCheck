# DiamondFsCheck
The Diamond kata done with Property-Based Testing in F#. Source code accompanying the [blog post about the same topic](http://blog.ploeh.dk/2015/01/10/diamond-kata-with-fscheck).

Ironically, over the course of two other blog posts, this kata code has now reaced a state where FsCheck is no longer in use

However, each blog post has an associated git tag, so if you want to see the FsCheck part of the code, you'll only need to check out the appropriate tag:

- *blog-post-1:* http://blog.ploeh.dk/2015/01/10/diamond-kata-with-fscheck
- *blog-post-2:* http://blog.ploeh.dk/2015/02/23/a-simpler-arbitrary-for-the-diamond-kata
- *blog-post-3:* http://blog.ploeh.dk/2015/02/23/property-based-testing-without-a-property-based-testing-framework
- *blog-post-3-updated:* http://blog.ploeh.dk/2015/02/23/property-based-testing-without-a-property-based-testing-framework

The *blog-post-3-updated* tag corresponds to an update to both this repository and the blog post istead, because it turned out that in all the excitement, I'd forgotten to add Unquote assertions as I left FsCheck (which just relies on the properties returning *true* or *false*).
