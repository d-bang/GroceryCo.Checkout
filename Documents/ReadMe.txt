Assumptions
-----------
1) We live in a land of unicorns and rainbows - so no tax.
2) Batch promotions and unit sales promotions can be combined but only 1 each
3) We're assuming both the product and promotions catalogs are up to date with out any overlap and there are no promotions the wouldn't be valid for the currentdate
4) Scan list contains GUIDs that are present for products in the product catalog, and there is one per line (carriage return separated)
5) No validation is necessary (see notes) for any input including the file paths


Limitations/design Choices
--------------------------
In the interest of time error handling and logging have been largely omitted, I would normally use log4net or somesuch 
tool for logging.

There are primarily happy path tests and they are relatively shallow. The testing could/should be more
robust. Accessors could have been created to test private methods but were not, and some classes should perhaps 
have been deconstructed/refactored due to the preponderance of private methods.


Notes
-----
I implemented some rudimentary data validation but something in the data deserialization process was getting
in the way, so in the interest of time I commented this bit out.

Expecting the list & catalog files to be there (not validating)

A truly robust solution would fail gracefully and or recover while executing, I've treated this
excercise as a demonstration of conceptual understanding as opposed to production readiness.
I apologize if you were expecting the latter.

When Testing put the list/catalog files in a location that keeps the file paths shorter
as all three are read in as one (comma separated).