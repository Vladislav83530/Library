# Library API
````
<summary>
Get all books Order by provided value (title or author)
GET https://{{baseUrl}}/api/books?order=author
</summary>
<param name="order"></param>
<returns>
Response
[{
    "id": "number",
    "title": "string",
    "author": "string",
    "rating": "decimal",          average rating
    "reviewsNumber": "number"     count of reviews
}]
</returns>
````
````
<summary>
Get top 10 books with high rating and number of reviews greater than 10
Filter books by specifying genre. 
Order by rating       
GET https://{{baseUrl}}/api/recommended?genre=horror
</summary>
<param name="genre"></param>
<returns>
Response
[{
    "id": "number",
    "title": "string",
    "author": "string",
    "rating": "decimal",          average rating
    "reviewsNumber": "number"     count of reviews
}]
</returns>
````
````
<summary>
Get book details with the list of reviews
GET https://{{baseUrl}}/api/books/{id}
</summary>
<param name="Id"></param>
<returns>
Response
{
    "id": "number",
    "title": "string",
    "author": "string",
    "cover": "string",
    "content": "string",
    "rating": "decimal",        average rating
    "reviews": [{
        "id": "number",
        "message": "string",
        "reviewer": "string",
    }]
}}
</returns>
````
````
<summary>
 Delete a book using a secret key
 DELETE https://{{baseUrl}}/api/books/{id}?secret=qwerty
</summary>
<param name="Id"></param>
<param name="secret"></param>
<returns></returns>
````
````
<summary>
<summary>
Save a new book
POST https://{{baseUrl}}/api/books/save
{
    "id": "number",             	
    "title": "string",
    "cover": "string",        save image as base64
    "content": "string",
    "genre": "string",
    "author": "string"
}
</summary>
<param name="book"></param>
<returns>
Response
  Id
</returns>
````
````
<summary>
Save a review for the book.
PUT https://{{baseUrl}}/api/books/{id}/review
{
    "message": "string",
    "reviewer": "string",
}
</summary>
<param name="id"></param>
<param name="review"></param>
<returns>
Response
  Id
</returns>
````
````
<summary>
Rate a book
PUT https://{{baseUrl}}/api/books/{id}/rate
{
    "score": "number"     score can be from 1 to 5
}
</summary>
<param name="id"></param>
<param name="rating"></param>
<returns></returns> 
````
