# QEngine
QEngine is Query Engine which dynamically constructs query from given string and executes it over a text file which acts as database.

# Developer's Guide
 - Comma separated file is parsed and a collection of objects is created dynamically based on the properties specified (first line) in the text file.
 - Querry string is parsed and query is built dynamically and applied to the collection created.

# User Guide
Application requires text file and database name as argument to accept query.
It supports the following types of query alone.
 - SELECT * FROM products WHERE store=2 
 - SELECT brand FROM products WHERE price > 600 
 - SELECT MAX(price) FROM products
 - SELECT UNIQ(store) FROM products WHERE in_stock=false
 - SELECT title FROM products WHERE in_stock=false AND brand=5 
 - SELECT title FROM products WHERE in_stock=false AND ( brand=5 OR store=2 )
 
 Note: Qury string is case sensitive.