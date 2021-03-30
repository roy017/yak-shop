# yak-shop

﻿This Project is yak shop, it is an ASP.NET Core Web Application.
In this project you can request what the yak herd will look like in the provided days.
You can also get information on the stock after provided days and see if you place an order if there is enough stock.

You can make Http Rest Get and Post calls, with the use of for example Postman, like:
- GET https://localhost:5001/Yak-Shop/herd/18 , which will give you the herd info at day 18
- GET https://localhost:5001/Yak-Shop/herd , which will give you the herd info at day 0
- POST https://localhost:5001/Yak-Shop/herd , which will add a yak to the herd, by providing a JSON, for example:
{
    "name": "Bob",
    "age": 3,
    "sex": "f",
    "ageLastShaved": 3
}
- GET https://localhost:5001/Yak-Shop/stock/17 , which will give you the stock info at day 17

Working with the program:
Clone the repository,
Be sure to have Dokcer installed,
Run the docker-compose.yml file in your PowerShell, to create your local server,
Check if your server is running,
Run the project, (be sure that yak-shopProject is the startup project)
