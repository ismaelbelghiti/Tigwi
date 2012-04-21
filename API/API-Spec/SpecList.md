Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 


#Get informations about a _List_

##Get accounts followed by the list
###Purpose
Obtain a number _n_ of accounts followed by list with id _idOfList_.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infolist/subscriptions/idOfList/numberOfSubscriptions
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

*Otherwise

    <Error/>

###Informations
* In **URL**, _idOfList_ is the id of the list whose informations you want to get
* In **URL**, _numberOfSubscriptions_ is the number of accounts you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of accounts returned (different from _numberOfSubscriptions_ if there are not enough accounts to provide).

##Get the list of accounts following a given list
###Purpose
Obtain a number _n_ of accounts following the given list with id _idOflist_.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infolist/subscribers/idOfList/numberOfFollowers
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

Erro Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:

    <Error/>

###Informations
* In **URL** _idOfList_ is the id of the list whose followers you want to get.
* In **URL**, _numberOfFollowers_ is the number of accounts you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of accounts returned (different from _numberOfFollowers_ if there are not enough accounts to provide).

##Get a list's owner informations
###Purpose
Obtain the name and id of list with id _idOfList_ 's owner.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infolist/owner/idOfList
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
* In **URL**, _idOfList_ is the id of the list whose owner you want to get.

##Get last messages sent to a list
###Purpose
Obtain a number _n_ of last messages sent to the list with id _idOfList_.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infolist/messages/idOfList/numberOfMessages
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

    <Messages Size="sizeOfList">
	    <Message> <!-- See below --> </Message>
	    <Message> ... </Message>
	    ...
	    <Message> ... </Message>
    </Messages>

Message format:

     <Message>
	     <Id> idOfMessage </Id>
	     <PostTime> timeOfPost </PostTime>
	     <Poster> nameOfUser </Poster>
	     <Content> content </Content>
     </Message>

Error Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:

    <Error/>

###Informations
* In **URL**, _idOfList_ is the id of the list whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough accounts to provide).


#Modifying a _list_

These methods require authentication. You must be authenticated as an user with appropriate autorization on the list you want to modify.

##Make a list suscribe to an account
###Purpose
For a list to add a suscription to a given account. Authentication required.

###HTTP method
*POST*
###URL
http://api.tigwi.com/modifylist/subscribeaccount/
###Request
    
    <SubscribeAccount>
        <List> idOfSuscriber </List>
        <Subscription> nameOfSubscription </Suscription>
    </SubscribeAccount>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>
	
###Informations
* You **must** be authenticated and authorized to use the owner of the list with id _idOfSuscriber_ to use this method.
* In **Request**, _idOfSuscriber_ is the id of the list who wants to follow the account _nameOfSubscription_.


##Delete an account from a list
###Purpose
Delete the suscription of the given account to the list. Authentication required.

###HTTP method
*POST*
###URL
http://api.tigwi.com/modifylist/unsubscribeaccount/{idOfList}/{accountId}
###Request
    
    <UnubscribeAccount>
        <List> idOfList </List>
        <Account> accountId </Account>
    </UnsubscribeAccount>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>


##Make an account follow a list
###Purpose
The given account follows the given list. Authentication required.

###HTTP method
*POST*
###URL
http://api.tigwi.com/modifylist/followlist/{idOfList}/{accountId}
###Request
    
    <FollowList>
        <List> idOflist </List>
        <Account> accountId </Account>
    </FollowList>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>

##Make an account unfollow a list
###Purpose
The given account does not follow the given list anymore. Authentication required.

###HTTP method
*POST*
###URL
http://api.tigwi.com/modifylist/unfollowlist/{idOfList}/{accountId}
###Request
    
    <UnfollowList>
        <List> idOflist </List>
        <Account> accountId </Account>
    </UnfollowList>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>