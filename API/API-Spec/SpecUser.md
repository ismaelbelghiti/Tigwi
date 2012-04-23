Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get inofrmation about an _user_

##Get an user's informations
###Purpose
Obtain main informations of user _userLogin_.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infouser/maininfo/userLogin  
or  
http://api.tigwi.com/infouser/maininfo/userId
###Request
_left empty_
###Response
General structure of the response :

    <Answer>
        <!-- Error Type -->
		<Content> 
            <!-- See below -->
        </Content> 
    </Answer>    
  
Content:
   
    <User>
        <Login> ... </Login>
        <Avatar> ... </Avatar>
        <Email> ... </Emain>
        <Id> ... </Id>
    </User>

Error type:  
* In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:
   
    <Error/>

###Informations
* In **URL**, _userLogin_ is the login of the user whose informations you want to get.
* In **URL**, _userId_ is the unique identifier of the user whose informations you want to get.

##Get the accounts where te user can post
###Purpose
Obtain a number _numberOfAccounts_ of accounts where user _userLogin_ can write.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infouser/authorizedaccounts/userLogin/numberOfAccounts
or  
http://api.tigwi.com/infouser/authorizedaccounts/userId/numberOfAccounts
###Request
_left empty_
###Response
General structure of the response :

    <Answer>
        <!-- Error Type -->
		<Content> 
            <!-- See below -->
        </Content> 
    </Answer>    
  
Content:

    <Accounts Size="sizeOfList">
	    <Account> <!-- See below --> </Account>
	    <Account> ... </Account>
	    ...
	    <Account> ... </Account>
    </Accounts>

Account format:

     <Account>
	     <Id> idOfAccount </Id>
	     <Name> nameOfAccount </Name>
         <Description> <!-- Description of the account --> </Description>
     </Account>

Error Type:  
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:

    <Error/>

###Informations
* In **URL**, _accountName_ is the name of the account whose subscribers you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose subscribers you want to get.
* In **URL**, _numberOfSubscribers_ is the number of subscribers you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of subscribers returned (different from _numberOfSubscribers_ if there are not enough subscribers to provide).

#Modifying an _user_