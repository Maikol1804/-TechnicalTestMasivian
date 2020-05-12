# Technical Test Masivian

This is technical exercise using C# .Net to Masivian.

## Problem

Implement an API that represents an online betting roulette.

## General comments:

This application was implemented using different clean code principles including the SOLID principles.

## Bet Types

There are two types of bets that are described in the following table:

|  Code |     Type    |
|-------|-------------|
|   1   | With Number |
|   2   | With Color  |

For the numerical type only the numbers from `0` to `36` are allowed and for the color type only the `black` or `red` color are allowed.

## Main Endpoints

* Create a roulette:

	`POST` {domain}/api/Roulette
	
	Response Example:
	```
	{
		"id": 1
	}
	```

* Open a roulette:

	`PUT` {domain}/api/Roulette/{rouletteId}/open
	
	Response Example:
	```
	{
		"code": "Ok",
		"message": "Roulette was opened"
	}
	```

* Create a bet:

	`POST` {domain}/api/Bet/
	
	Body Example 1:
	```
	{
		"player": {
			"id": 1	
		 },
		"roulette": {
			"id": 1
		},
		"betType": {
			"code": "1",
			"value": "21" 
		},
		"amount": 1000
	}
	```
	
	Response Example 1:
	```
	{
		"player": {
			"id": 1
		},
		"roulette": {
			"isOpen": true,
			"id": 1
		},
		"betType": {
			"code": "1",
			"value": "21",
			"id": 1
		},
		"amount": 1000,
		"id": 1
	}
	```

	Body Example 2:
	```
	{
		"player": {
			"id": 5	
		 },
		"roulette": {
			"id": 1
		},
		"betType": {
			"code": "2",
			"value": "Black" 
		},
		"amount": 500
	}
	```
	
	Response Example 1:
	```
	{
		"player": {
			"id": 5
		},
		"roulette": {
			"isOpen": true,
			"id": 1
		},
		"betType": {
			"code": "2",
			"value": "Black",
			"id": 2
		},
		"amount": 500,
		"id": 2
	}
	```

* Close a roulette with your bets:

	`PUT` {domain}/api/Roulette/{rouletteId}/close
	
	Response Example:
	```
	[
		{
			"player": {
				"id": 1
			},
			"roulette": {
				"isOpen": false,
				"id": 1
			},
			"betType": {
				"code": "1",
				"value": "21",
				"id": 1
			},
			"amount": 1000,
			"id": 1
		},
		{
			"player": {
				"id": 5
			},
			"roulette": {
				"isOpen": false,
				"id": 1
			},
			"betType": {
				"code": "2",
				"value": "Black",
				"id": 2
			},
			"amount": 500,
			"id": 2
		}
	]
	```

* Get all roulettes:

	`GET` {domain}/api/Roulette
	
	Response Example:
	```
	[
		{
			"isOpen": false,
			"id": 1
		},
		{
			"isOpen": true,
			"id": 2
		}
	]
	```

Additionally [here](RouletteWebApi) you can find a Postman collection to consume the services or watch the swagger when the API runs.

That's it :+1:
