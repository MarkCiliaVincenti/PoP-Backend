# PoP-Backend
A more secure, scalable, and modern backend infrastructure to support the Pencils of Promise gala

### Auth
We use a basic auth schema with username and password sent encrypted in the header.

## APIs

### Accounting

#### GET accounting/total
```json
0.00
```
#### POST accounting/finalize
- Creates invoices for all outstanding donation and bid payments for all users
- BODY contains a single string password for triggering the action
```json
""
```

#### POST accounting/pledge
- Create an incomplete payment to be invoiced later
```json
{
    "email": "",
    "amount": 0.00,
    "auctionId": 0,
    "description": ""
}
```

### Auctions

#### GET auction
```json
[
  {
    "id": 1,
    "auctionTypeId": 1,
    "title": "fake",
    "description": "test",
    "restrictions": "sdaddd",
    "isActive": true,
    "amount": 206.00,
    "created": "2021-10-13T21:48:14.767",
    "imageUrl": null
  }
]

```

#### GET auction/{typeId}
- Get all auctions under a specific type
```json
[
  {
    "id": 1,
    "auctionTypeId": 1,
    "title": "fake",
    "description": "test",
    "restrictions": "sdaddd",
    "isActive": true,
    "amount": 206.00,
    "created": "2021-10-13T21:48:14.767",
    "imageUrl": null
  }
]

```

#### GET auction/highestbidoftype/{typeId}
- Gets highest bid information for all auctions of type
```json
[
    {
        "id": 14,
        "auctionId": 2,
        "amount": 10.00,
        "email": "d@gmail.com",
        "timestamp": "2021-10-13T21:16:01.517"
    }
]
```

#### GET auction/highestbid/{auctionId}
- Gets highest bid for specific auction
```json
[
    {
        "id": 14,
        "auctionId": 2,
        "amount": 10.00,
        "email": "d@gmail.com",
        "timestamp": "2021-10-13T21:16:01.517"
    }
]
```

#### POST auction/bid/{auctionId}
- Creates invoices for all outstanding donation and bid payments for all users
- BODY contains a single string password for triggering the action
```json
{
  "email": "",
  "Amount": 0.0
}
```
