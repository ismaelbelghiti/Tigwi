Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get informations about an _account_

##Get someone's recently sent messages
###Purpose
Obtain a number _n_ of the account _accountName_ 's last posted messages
###HTTP method
*GET*
###URL
http://api.tigwi.com/account/messages/accountName/numberOfMessages  
or  
http://api.tigwi.com/accountbyid/messages/accountId/numberOfMessages
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
* In **URL**, _accountId_ is the unique identifier of the account whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).

##Get someone's public subscriptions (accounts) list
###Purpose
Obtain a number _numberOfSubscriptions_ of accounts in any of the account _accountName_'s followed public lists. 
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/account/publiclysubscribedsaccounts/accountName/numberOfSubscriptions  
or  
http://api.tigwi.com/accountbyid/publiclysubscribedaccounts/accountId/numberOfSubscriptions
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
http://api.tigwi.com/account/subscribedaccounts/accountName/numberOfSubscriptions  
or  
http://api.tigwi.com/accountbyid/subscribedaccounts/accountId/numberOfSubscriptions
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
* You **must** be authenticated as an authorized user of account  _accountName_ to have access to these informations. 
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
http://api.tigwi.com/account/subscribersaccounts/accountName/numberOfSubscribers  
or  
http://api.tigwi.com/accountbyid/subscribersaccounts/accountId/numberOfSubscribers
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
Obtain a number _numberOfLists_ of the account _accountName_ 's followed public lists.
No particular order provided.
###HTTP method
*GET*
###URL
http://api.tigwi.com/account/subscribedpubliclists/accountName/numberOfLists  
or  
http://api.tigwi.com/accountbyid/subscribedpubliclists/accountId/numberOfLists
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
* In **URL**, _accountName_ is the name of the account whose publicly followed lists you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose publicly followed lists you want to get.
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
http://api.tigwi.com/account/subscribedlists/accountName/numberOfLists  
or  
http://api.tigwi.com/accountbyid/subscribedlists/accountId/numberOfLists
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
* You **must** be authenticated as an authorized user of account  _accountName_ to use this method.
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
http://api.tigwi.com/account/subscriberlists/accountName/numberOfSubscribers  
or  
http://api.tigwi.com/accountbyid/subscriberlists/accountId/numberOfSubscribers
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
Obtain a number _numberOfLists_ of the account _accountName_ 's owned public lists.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/account/ownedpubliclists/accountName/numberOfLists  
or  
http://api.tigwi.com/accountbyid/ownedpubliclists/accountId/numberOfLists
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
No particular order provided.
Authentication required.
###HTTP method
*GET*
###URL
http://api.tigwi.com/account/ownedlists/accountName/numberOfLists  
or  
http://api.tigwi.com/accountbyid/ownedlists/accountId/numberOfLists
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
* You **must** be authenticated as an authorized user of account _accountName_ to have access to these informations. 
* In **URL**, _accountName_ is the name of the account whose lists you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).


##Get an account's main informations
###Purpose
Obtain the account _accountName_ 's main informations.
Authentication required.
###HTTP method
*GET*
###URL
http://api.tigwi.com/account/maininfo/accountName  
or  
http://api.tigwi.com/accountbyid/maininfo/accountId
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
* You **must** be authenticated as an authorized user of account  _accountName_ to access this information.
* In **URL**, _accountName_ is the name of the account whose main informations you want to get.
* In **URL**, _accountId_ is the name of the account whose main informations you want to get.

#Modifying an _account_

These methods require authentication. You must be authenticated as a _user_ with permissions to use the _account_ you want to modify.

##Post a message
###Purpose
For someone to send a message on an authorized account. 
Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/account/write
###Request

    <Write>
	   <AccountName> accountName </AccountName>
       // or you can use
       <AccountId> accountId </AccountId>
 
       <Message>
            <Content> <!-- your message --> </Content>
       </Message>
    </Write>

###Response
General structure of the response :

    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    

Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated as an authorized user of account _nameOfAccount_ to post a message.
* In **Request**, the size of your message is limited to 140 characters, but this limit is tested by the server. It raises an error if the message is too long.
* In **Request**, _accountName_ is the name of the account where you intend to post a message.
* In **Request**, _accountId_ is the unique identifier of the account where you intend to post a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).
* In **Response**, the identifier provided is the message created's one.

##Copy a message
###Purpose
For someone to copy a message on an authorized account. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/account/copy
###Request

    <Copy>
	   <AccountName> nameOfAccount </AccountName>
       // or you can use
       <AccountId> idOfAccount </AccountId>
 
       <MessageId> idOfMessage </MessageId>
    </Copy>

###Response
General structure of the response :


    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    


Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated as an authorized user of account _nameOfAccount_ to copy a message.
* In **Request**, _accountName_ is the name of the account where you intend to copy a message.
* In **Request**, _accountId_ is the unique identifier of the account where you intend to copy a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).
* In **Response**, the identifier provided is the message created's one (copying a message means creating a new one with the same content).



##Remove a message
###Purpose
For someone to remove a previously posted message on an authorized account. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/account/delete
###Request

    <Delete>
        <AccountName> accountName </AccountName>
        // or you can use
        <AccountId> accountId </AccountId>

        <MessageId> messageId </MessageId>
    </Delete>

###Response
General structure of the response :


    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    


Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated as an authorized user of account _accountName_ to remove a message.
* In **Request**, _accountName_ is the name of the account where you intend to delete a message.
* In **Request**, _accountId_ is the unique identifier of the account where you intend to delete a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).

##Add a message to an account's favorite
###Purpose
To tag a message as one of _accountName_'s favorites. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/account/tag
###Request

    <Tag>
        <AccountName> accountName </AccountName>
        // or you can use
        <AccountId> accountId </AccountId>

        <MessageId> messageId </MessageId>
    </Tag>

###Response
General structure of the response :


    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    


Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated as an authorized user of _accountName_ to tag a message.
* In **Request**, _accountName_ is the name of the account where you intend to tag a message.
* In **Request**, _accountId_ is the unique identifier of the account where you intend to tag a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).

##Remove a message from an account's favorite
###Purpose
To untag a message from _accountName_'s favorites. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/account/untag
###Request

    <Untag>
        <AccountName> accountName </AccountName>
        // or you can use
        <AccountId> accountId </AccountId>

        <MessageId> messageId </MessageId>
    </Untag>

###Response
General structure of the response :


    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    


Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated as an authorized user of account _accountName_ to remove a tagged message.
* In **Request**, _accountName_ is the name of the account where you intend to untag a message.
* In **Request**, _accountId_ is the unique identifier of the account where you intend to untag a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).

##Create a list
###Purpose
For someone to create a new, empty list.
Authentication required.

###HTTP method
*POST*

###URL
http://api.tigwi.com/account/createlist/

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
General structure of the response :


    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    


Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated and authorized to use account _nameOfSubscriber_ to use this method.

* In **Request**, _nameOfSubscriber_ is the name of the account who wants to create the list _nameOfList_.

* In **Request**, _nameOfList_ is the name you want to give to the new list.

* In **Request**, _aQuickDescription_ is a short text to remember what the list is about.

* In **Request**, _privateSetting_ value must be _false_ if you want the new list to be public or _true_ if only you can see that list.

##Make an account subscribe to a list
###Purpose
For someone to distantly subscribe to a list. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/account/subscribelist/
###Request
    
    <SubscribeList>
        <AccountName> nameOfSubscriber </AccountName>
        // or you can use
        <AccountId> idOdSubscriber </AccountId>

        <Subscription> idOfSubscription </Suscription>
    </SubscribeList>

###Response
General structure of the response :


    <Answer>
        <!-- Error Type -->
        <Content xsi:type="ObjectCreated" 
           Id="UniqueIdentifierOfCreatedObject" />
    </Answer>    


Error type:  
*In case an error occurs:


    <Error Code="codeOfError"/>


*Otherwise:
   
    <Error/>


###Informations
* You **must** be authenticated as an authorized user of account _nameOfSubscriber_ to use this method.
* In **Request**, _nameOfSubscriber_ is the name of the account who wants to follow the list _idOfSubscription_.
* In **Request**, _idOfSubscriber_ is the unique identifier of the account who wants to follow the list _idOfSubscription_.

#Get informations about a _List_

##Get accounts followed by the list
###Purpose
Obtain a number _n_ of accounts followed by list with id _idOfList_.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/list/subscriptions/idOfList/numberOfSubscriptions
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
http://api.tigwi.com/list/subscribers/idOfList/numberOfFollowers
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
http://api.tigwi.com/list/owner/idOfList
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
http://api.tigwi.com/list/messages/idOfList/numberOfMessages
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
http://api.tigwi.com/list/subscribeaccount/
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
http://api.tigwi.com/list/unsubscribeaccount/
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

#Get inofrmation about an _user_

##Get an user's informations
###Purpose
Obtain main informations of user _userLogin_.
###HTTP method
*GET*
###URL
http://api.tigwi.com/user/maininfo/userLogin  
or  
http://api.tigwi.com/userbyid/maininfo/userId
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
