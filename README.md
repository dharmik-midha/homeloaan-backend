# How to Run The Project

To run the project, please follow the below steps:

1. Clone the project from the repository.

2. Delete the old SQL database if present on your system with the name "HomeLoan".
    a. Open SSMS (Microsoft SQL Server Management Studio).
    b. Make sure to check the checkbox for "close existing connections" while deleting the old database.

3. Go to the package manager console in Visual Studio.

4. Select "DataAccess" as default.

5. Run the following command: `Add-Migration init`.

6. Run the following command: `Update-Database`.

7. Start the project.

8. Swagger will launch, and you can access the API endpoints.

If you face any issues while running the project, please reach out to the development team for assistance.

# Authentication
The api uses jwt token based authentication with token expiration time of 5 hours.
There can be users of 2 types either Advisor or User.
By default creating / registering a user from APIs will create a user of Type User.
The System contains one pre registered Advisor having credentials
<b>email : Advisor1@email.com & password : "Abcd@1234"</b>

# Accessibility of APIs

Diffrent API endpoints require diffrent levels of Authorization

#### Anonymous : POST/api/Auth/signup
Anyone can signup without any restrictions
#### Anonymous : POST/api/Auth/signin
Anyone can signin using email and password without any restrictions
#### User/Advisor : PUT/api/Auth
Either User or Advisor can cange their password using their old password after signin

<br><br>

#### User : GET/api/Collateral
User can get all of his collaterals after signin

#### User : POST/api/Collateral
User can create a new collateral after signin

#### User : PUT/api/Collateral
User can modify a collateral after signin

#### User : DELETE/api/Collateral
User can delete a collateral after signin

<br><br>

#### User : POST/api/Loan
User can create a new loan after signin

#### User : GET/api/Loan
User can see all his loans after signin

#### User : POST/api/Loan/{loanId}
User can apply a specific loan using loanId after signin

#### User : PATCH/api/Loan/{loanId}
User can edit a loan if its in created state using its loanId after signin

#### User : GET/api/Loan/{loanId}
User can get a loan using loan id after signin

#### Advisor : GET/api/Loan/get-all
Advisor can get all of users loan after signin

#### User : POST/api/Loan/{loanId}/collateral
User can add a collateral to loan using loanId and coillateralId after signin

#### User : GET/api/Loan/{loanId}/collateral
User can get all collaterals in a loan after signin

#### User : DELETE/api/Loan/{loanId}/collateral/{collateralId}
User can delete or remove a collateral from a loan using loanId and CollateralId after signin

#### Advisor : POST/api/Loan/{loanId}/state
Advisor can change the state of the loan after signin.<br>
Advisor can change the loan application state from:
1. Rejected to (In progress)
2. Recommented to (Rejected, In Progress)
3. In Progress to (Accepted, Rejected, Recommended)

<br><br>

#### Anonymous : GET/api/Promotion
Anyone can get the current active promotion

#### Advisor : POST/api/Promotion
Advisor can add new Promotions.

<br>
<br>

#### Anonymous : GET/api/Static/country
Anyone can get the list of all countries.

#### Advisor : POST/api/Static/country
Advisor can add a country

#### Anonymous : GET/api/Static/country/{countryId}
Anyone can get a country by its id

#### Advisor : PUT/api/Static/country/{countryId}
Advisor can Edit a country

#### Advisor : DELETE/api/Static/country/{countryId}
Advisor can delete a country

#### Anonymous : GET/api/Static/country/{countryId}/state
Anyone can get the list of all states in a country.

#### Advisor : POST/api/Static/country/{countryId}/state
Advisor can add a state in a country

#### Anonymous : GET/api/Static/state/{stateId}
Anyone can get a state by its id

#### Advisor : PUT/api/Static/state/{stateId}
Advisor can Edit a state

#### Advisor : DELETE/api/Static/state/{stateId}
Advisor can delete a state

#### Anonymous : GET/api/Static/state/{stateId}/city
Anyone can get the list of all cities in a state.

#### Advisor : POST/api/Static/state/{stateId}/city
Advisor can add a city in a state

#### Anonymous : GET/api/Static/city/{cityId}
Anyone can get a city by its id

#### Advisor : PUT/api/Static/city/{cityId}
Advisor can Edit a city

#### Advisor : DELETE/api/Static/city/{cityId}
Advisor can delete a city



# Enums
The Api uses enums for constants so the requests require correct mapped integer for it to work.
Here is a list of all the Enums.

|<b>Collateral Types</b>||  
| ------ | ------ |
| Insurance_Policy | 0 |
| Gold | 1 | 
| Stock | 2 |
| Property | 3 |  

| <b>Loan State</b>|  |
| ------ | ------ |
| Created | 0 |
| InProgress | 1 | 
| Accepted | 2 |
| Rejected | 3 | 
| Recommended | 4 |

| <b>LoanStatus	</b>|  |
| ------ | ------ |
| RED | 0 |
| YELLOW | 1 | 
| GREEN | 2 |

| <b>PromotionTypes</b>|  |
| ------ | ------ |
| A | 0 |
| B | 1 | 
| C | 2 |

# Values

| <b>Collateral Conversion Value</b>|  |
| ------ | ------ |
| Insurance_Policy | 80% |
| Gold | 75% | 
| Stock | 50% |
| Property | 80% |

| <b>LoanStatus Value	</b>|  |
| ------ | ------ |
| RED | <= 40% |
| YELLOW | <= 70% | 
| GREEN | > 70% |

