
# __*BandTracker*__
#### __*By: Derek Villars*__

### *Setup/Installation Requirements:*
 After Cloning this repository to your computer you need to open the index.html file and the website should open up in your browser.

in SQLCMD:

    CREATE DATABASE band_tracker
    GO
    USE band_tracker
    GO
    CREATE TABLE venues
    (
    id INT IDENTITY(1,1),
    name VARCHAR(255)
    )
    GO
    CREATE TABLE bands
    (
    id INT IDENTITY(1,1),
    name VARCHAR(255)
    )
    GO
    CREATE TABLE bands_venues
    (
    id INT IDENTITY(1,1),
    band_id INT,
    venue_id INT
    )
    GO

### __*Specifications:*__

1. Build full CRUD functionality for Venues. Create, Read (show all, show single), Update, Delete.
  - <!-- TODO -->
- Allow a user to create Bands that have played at a Venue. Don't worry about building out updating or deleting for bands.
  - <!-- TODO -->
- There is a many-to-many relationship between bands and concert venues, so a venue can host many bands, and a band can play at many venues. Create a join table to store these relationships.
  - <!-- TODO -->
- When a user is viewing a single concert venue, list out all of the bands that have played at that venue so far and allow them to add a band to that venue. Create a method to get the bands who have played at a venue, and use a join statement in it.
  - <!-- TODO -->
- When a user is viewing a single Band, list out all of the Venues that have hosted that band and allow them to add a Venue to that Band. Use a join statement in this method too.
  - <!-- TODO -->

<!-- TODO:fix
### __Example Data:__

###### Clent Examples (figure 1)
| Client Id | Name  | Stylists Id |
| --------- | ----- | ----------- |
| 1         | Bob   | 2           |
| 2         | Keven | 1           |
| 3         | Jill  | 3           |
| 4         | Emma  | 2           |
| 5         | Frank | 3           |

###### Band Examples (figure 2)

| band_id | name |
| ------- | ---- |
| 1       | one1 |
| 2       | pasX |
| 3       | boOm |

###### Venue Examples (figure 3)

| venue_d | name |
| ------- | ---- |
| 1       |  |
| 2       | Joey |
| 3       | John | -->



### *Support and Contact:*
If you have any questions for me or have any problems, please email me at derekvillars@yahoo.com.

Copyright (c) 2017 __Derek Villars__
