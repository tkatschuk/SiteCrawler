# Generic Site Crawler

## Task

Develope a generic site crawler library, that provides basic services to traverse a complete site tree. Given a start link it visits each page of the site, i.e. all pages that are reachable via one or more hops from the startpage within the same domain. The caller can execute an arbitrary action on each of the pages.

#### Deliverables

* Develope a basic crawler component as described above
* A sample console application that uses the crawler component to save all site pages for a given url as static files to the file system.