SELECT Products.Name as ProductsName, Categories.Name as CategoryName
FROM Products
         LEFT JOIN Categories on Products.CategoryID = Categories.CategoryID