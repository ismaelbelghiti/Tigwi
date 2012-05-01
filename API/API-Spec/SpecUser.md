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
* In **URL**, _userLogin_ is the login of the user whose authorized accounts you want to get.
* In **URL**, _userId_ is the unique identifier of the user whose authorized accounts you want to get.
* In **URL**, _numberOfAccounts_ is the number of accounts you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of accounts returned (different from _numberOfAccounts_ if there are not enough accounts to provide).

#Modifying an _user_

##Change an user's email
###Purpose
If you're authenticated as _userLogin_, you can change the email provided in you personnal informations.
###HTTP method
*POST*
###URL
http://api.tigwi.com/modifyuser/changeemail
###Request
    
    <ChangeInfo>
        <UserLogin> userLogin </UseLogin>
        // or you can use
        <UserId> userID </UserId>

        <Info> <!-- New mail address --> </Info>
    </ChangeInfo>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated as _userLogin_ to use this method.
* In **Request**, _userLogin_ is the login of the user whose email you want to change.
* In **Request**, _userId_ is the unique identifier of the user whose email you want to change. If you use both `<UserLogin>` and `<UserId>`, only `<UserId>` will be taken into account.

##Change an user's email
###Purpose
If you're authenticated as _userLogin_, you can change the avatar provided in you personnal informations.
###HTTP method
*POST*
###URL
http://api.tigwi.com/modifyuser/changeavatar
###Request
    
    <ChangeInfo>
        <UserLogin> userLogin </UseLogin>
        // or you can use
        <UserId> userID </UserId>

        <Info> <!-- New avatar --> </Info>
    </ChangeInfo>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated as _userLogin_ to use this method.
* In **Request**, _userLogin_ is the login of the user whose avatar you want to change.
* In **Request**, _userId_ is the unique identifier of the user whose avatar you want to change. If you use both `<UserLogin>` and `<UserId>`, only `<UserId>` will be taken into account.


##Change an user's password
###Purpose
If you're authenticated as _userLogin_, you can change the password required for authentifcation.
###HTTP method
*POST*
###URL
http://api.tigwi.com/modifyuser/changepassword
###Request
    
    <ChangePassword>
        <UserLogin> userLogin </UseLogin>
        // or you can use
        <UserId> userID </UserId>

        <OldPassword> <!-- Old Password --> </OldPassword>
        <NewPassword> <!-- New Password --> </NewPassword>
    </ChangePassword>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated as _userLogin_ to use this method.
* In **Request**, _userLogin_ is the login of the user whose password you want to change.
* In **Request**, _userId_ is the unique identifier of the user whose email you want to change. If you use both `<UserLogin>` and `<UserId>`, only `<UserId>` will be taken into account.
* In **Request**, you have to provide your old password, even if you're already authenticated, to be sure that no one is changing your password against your will.

##Create an account
###Purpose
If you're authenticated as _userLogin_, you can create a new account with _userLogin_ defined as administrator.
###HTTP method
*POST*
###URL
http://api.tigwi.com/modifyuser/createaccount
###Request
    
    <CreateAccount>
        <UserLogin> userLogin </UseLogin>
        // or you can use
        <UserId> userID </UserId>

        <AccountName> accountName </accountName>
        <Description> <!-- Description of the account--> </Description>
    </CreateAccount>

###Response
General structure of the response :

    <Answer>
        <!-- Error Type -->
		<Content> 
            <!-- See below -->
        </Content> 
    </Answer>    
  
Content:

     <ObjectCreated Id="UniqueIdentifierOfCreatedObject"/>

Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>



###Informations
* You **must** be authenticated as _userLogin_ to use this method.
* In **Request**, _userLogin_ is the login of the user whose email you want to change.
* In **Request**, _userId_ is the unique identifier of the user who wants to create an account. If you use both `<UserLogin>` and `<UserId>`, only `<UserId>` will be taken into account.
* In **Response**, you get the information about the account just created.