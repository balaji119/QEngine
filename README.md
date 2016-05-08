# QEngine
QEngine is Query Engine written in C# which dynamically constructs query from given string and executes it over a text file which acts as database.

# Developer's Guide
 - QEngine.cs has the Main function.
 - Comma separated file is parsed and a collection of objects is created dynamically based on the properties specified (first line) in the text file.
 - Querry string is parsed and query is built dynamically and applied to the collection created.

# User Guide
Application requires text file path and database name as argument to load.
Eg. QEngine.exe E:\products.csv products
Once loaded, you can query continuously. Type "exit" or "quit" to quit the application.

QEngine supports the following types of query alone.
 - SELECT * FROM products WHERE store=2 
 - SELECT brand FROM products WHERE price > 600 
 - SELECT MAX(price) FROM products
 - SELECT UNIQ(store) FROM products WHERE in_stock=false
 - SELECT title FROM products WHERE in_stock=false AND brand=5 
 - SELECT title FROM products WHERE in_stock=false AND ( brand=5 OR store=2 )
 
 Note: Query string is case sensitive.