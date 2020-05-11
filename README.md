# Technical Test Masivian

This is technical exercise using C# .Net to Masivian.

## Problem

Implement an API that represents an online betting roulette.

## Bet Types

There are two types of bets that are described in the following table:

|  Code |     Type    |
|-------|-------------|
|   1   | With Number |
|   0   | With Color  |

## Main Endpoints

* Create a roulette
`POST` {domain}/api/Roulette
Body:
{}
Response:
{}

* Open a roulette
`PUT` {domain}/api/Roulette/{rouletteId}/open
Body Example:
{}
Response Example
{}

* Create a bet
`POST` {domain}/api/Bet/
Body Example:
{}
Response Example
{}

* Close a roulette
`PUT` {domain}/api/Roulette/{rouletteId}/close
Body Example:
{}
Response Example
{}

* Get all roulettes
`GET` {domain}/api/Roulette
Response Example:
{}

###### General comments:

This application was implemented using different clean code principles including the SOLID principles.


That's it :+1:
