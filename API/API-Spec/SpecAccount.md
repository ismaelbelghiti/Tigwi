Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get informations about an _account_

##Get someone's recently sent messages
###Purpose
Obtain a number _n_ of the account _accountName_ 's last posted messages
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/messages/accountName/numberOfMessages  
or  
http://api.tigwi.com/infoaccount/messages/accountId/numberOfMessages
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

Error type:
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>

###Informations
* In **URL**, _accountName_ is the name of the account whose messages you want to get.
* In **URL**, _accountId is the unique identifier of the account whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).

##Get someone's public subscriptions (accounts) list
###Purpose
Obtain a number _numberOfSubscriptions_ of accounts in any of the account _accountName_'s followed public lists. 
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/publicsubscriptionsaccounts/accountName/numberOfSubscriptions  
or  
http://api.tigwi.com/infoaccount/publicsubscriptionsaccounts/accountId/numberOfSubscriptions
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
	    <Acount> ... </Account>
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
* In **URL**, _accountName_ is the name of the account whose subscriptions you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose subscriptions you want to get.
* In **URL**, _numberOfSubscriptions_ is the number of subscriptions you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of subscription returned (different from _numberOfSubscriptions_ if there are not enough subscriptions to provide).
* You will only receive subscriptions from lists that the owner has declared public.


##Get someone's subscriptions (accounts) list
###Purpose
Obtain a number _numberOfSubscriptions_ of accounts in any of the account _accountName_'s followed lists.
No particular order provided.
Authentication required.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/subscriptionsaccounts/accountName/numberOfSubscriptions  
or  
http://api.tigwi.com/infoaccount/subscriptionsaccounts/accountId/numberOfSubscriptions
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
	    <Acount> ... </Account>
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
* You **must** be authenticated as _accountName_ to have access to these informations. 
* In **URL**, _accountName_ is the name of the account whose subscriptions you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose subscriptions you want to get.
* In **URL**, _numberOfSubscriptions_ is the number of subscriptions you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of subscription returned (different from _numberOfSubscriptions_ if there are not enough subscriptions to provide).


##Get someone's subscribers (accounts)
###Purpose
Obtain a number _numberOfSubsribers_ of accounts that subscribed to any of account _accountName_ owned public lists.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/subscribersaccounts/accountName/numberOfSubscribers  
or  
http://api.tigwi.com/infoaccount/subscribersaccounts/accountId/numberOfSubscribers
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

##Get an account's followed public lists
###Purpose
Obtain a number _n_ of the account _accountName_ 's followed public lists.
No particular order provided.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/subscribedpublic/accountName/numberOfLists  
or  
http://api.tigwi.com/infoaccount/subscribedpublic/accountId/numberOfLists
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

    <Lists Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </Lists>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

Error Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:

    <Error/>

###Informations
* In **URL**, _accountName_ is the name of the account whose followed lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).
* You will only receive lists that the owner has declared public.


##Get an account's followed lists
###Purpose
Obtain a number _n_ of the account _accountName_ 's followed lists, either public or private.
No particular order provided.
Authentication required.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/subscribed/accountName/numberOfLists  
or  
http://api.tigwi.com/infoaccount/subscribed/accountId/numberOfLists
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

    <Lists Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </Lists>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

Error Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:

    <Error/>

###Informations
* You **must** be authenticated and authorize to use _accountName_ account to use this method.
* In **URL**, _accountName_ is the name of the account whose followed lists you want to get.
* In **URL**, _accountId_ is the unique Identifier of the account whose followed lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).

##Get someone's subscribers (lists)
###Purpose
Obtain a number _numberOfSubsribers_ of public lists that subscribed to account _accountName_ .
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/subscribers/accountName/numberOfSubscribers  
or  
http://api.tigwi.com/infoaccount/subscribers/accountId/numberOfSubscribers
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

    <Lists Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </Lists>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

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
* There is no way to get private lists who subscribed to an account.

##Get an account's owned public lists
###Purpose
Obtain a number _n_ of the account _accountName_ 's owned public lists.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/ownedpubliclists/accountName/numberOfLists  
or  
http://api.tigwi.com/infoaccount/ownedpubliclists/accountId/numberOfLists
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

    <Lists Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </Lists>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

Error Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:
   
    <Error/>

###Informations
* In **URL**, _accountName_ is the name of the account whose lists you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).
* You will only receive lists that the owner has declared public.


##Get an account's owned lists
###Purpose
Obtain a number _n_ of the account _accountName_ 's owned lists, either public or private.
No particular order provided. Authentication required.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/ownedlists/accountName/numberOfLists  
or  
http://api.tigwi.com/infoaccount/ownedlists/accountId/numberOfLists
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

    <Lists Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </Lists>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

Error Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise

    <Error/>

###Informations
* You **must** be authenticated as _accountName_ to have access to these informations. 
* In **URL**, _accountName_ is the name of the account whose lists you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).


##Get an account's main informations
###Purpose
Obtain the account _accountName_ 's unique id.
You need to be authenticated and authorize to use this account to get its id.
###HTTP method
*GET*
###URL
http://api.tigwi.com/infoaccount/main/accountName
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
         <Description> DecriptionOfAccount </Description>
     </Account>

Error Type:
*In case an error occurs:

    <Error Code="codeOfError"/>

*Otherwise:
   
    <Error/>

###Informations
* You **must** be authenticated as an authorized user of _accountName_ to access this information.
* In **URL**, _accountName_ is the name of the account whose main informations you want to get.
* In **URL**, _accountId_ is the name of the account whose main informations you want to get.




#Modifying an _account_

These methods require authentication. You must be authenticated as a _user_ with permissions to use the _account_ you want to modify.

##Post a message
###Purpose
For someone to send a message on an authorized account. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/modifyaccount/write
###Request

    <Write>
       <User> nameOfUser </User>
	   <Account> nameOfAccount </Account> 
       <Message>
            <Content> <!-- your message --> </Content>
       </Message>
    </Write>

###Response
In case an error occurs

    <Error Code="codeOfError"/>


If no error occurs

    <Error/>

###Informations
* You **must** be authenticated as _nameofUser_ and authorized to use _nameOfAccount_ to post a message.
* In **Request**, the size of your message is limited to 140 characters, but this limit is tested by the server. It raises an error if the message is too long.

##Create a list
###Purpose
For someone to create a new, empty list. Authentication required.

###HTTP method
*POST*

###URL
http://api.tigwi.com/modifyaccount/createlist/

###Request
	<CreateList>
		<Account> nameOfSubscriber </Account>
		<ListInfo>
			<Name> nameOfList </Name>
			<Description> aQuickDescription </Description>
			<isPrivate> privateSetting </isPrivate>
		</ListInfo>
	</CreateList>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated and authorized to use _nameOfSubscriber_ to use this method.

* In **Request**, _nameOfSubscriber_ is the name of the account who wants to follow the list _nameOfSubscription_.

* In **Request**, _nameOfList_ is the name you want to give to the new list.

* In **Request**, _aRapidDescription_ is a short text to remember what the list is about.

* In **Request**, _privateSetting_ value must be _false_ if you want the new list to be public or _true_ if only you can see that list.

##Make an account subscribe to a list
###Purpose
For someone to distantly subscribe to a list. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/modifyaccount/subscribelist/
###Request
    
    <SubscribeList>
        <Account> nameOfSubscriber </Account>
        <Subscription> idOfSubscription </Suscription>
    </SubscribeList>

###Response
In case an error occurs

    <Error Code="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated and authorized to use the account _nameOfSubscriber_ to use this method.
* In **Request**, _nameOfSubscriber_ is the name of the account who wants to follow the list _idOfSubscription_.
