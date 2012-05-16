Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#General presentation of the Tigwi API

##What you can do with our API

If you're a smartphone developper, you may be willing to make your own application to access Tigwi from everywhere. The simpliest way to do that is to use our API. Your software will be able to join our servers to get recent sent messages, see who's following who and even to post some content.

Our API provides a lot of functions. You can even create new lists, follow new people by adding them to one of your lists, follow public lists made by a complete stranger or tag messages you like the most.

If you're a webmaster, you would like to show your last posts ? You can use our API to make a rather simple javascript application doing that.

##Note about authentication

Some of our ressources require authentication. This is the case if you use any with a POST verb but also if you want to access to private data such as an user email, or an account private lists.

To authenticate, you need to send an HTTP cookie named _key_ whose value should be the user login for the moment, along with the request.
This means that if you're writing requests manually you should write something like this :

    Cookie: key=George_Bush_login


#Get informations about an _account_

##Read last messages wrote by someone

###Purpose

Get the number you want to of an account last messages.

###HTTP method

GET

###URL

http://api.tigwi.com/account/messages/_accountName_/_numberOfMessages_
http://api.tigwi.com/account/messages/name=_accountName_/_numberOfMessages_
http://api.tigwi.com/account/messages/id=_accountId_/_numberOfMessages_

###Response

Example of a response if you requested for 
http://api.tigwi.com/account/messages/George_bush/30

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Messages" Size="4">
      <Message>
       <Id>1ccacf01-d531-4062-930a-7a2a9fad9559</Id>
       <PostTime>2012-05-16T12:37:23.9819744</PostTime>
       <Poster>George_Bush</Poster>
       <Content>Tigwi is great ! This is my first message. I've just joined. I encourage you to do so.</Content>
      </Message>
      <Message>
       <Id>d89814c5-b61f-4820-b8c0-49fb179649f8</Id>
       <PostTime>2012-05-16T12:38:13.0275938</PostTime>
       <Poster>George_Bush</Poster>
       <Content>Well, there doesn't seem to be a lot of people that heard of my last advice.</Content>
      </Message>
      <Message>
       <Id>a1624bfc-3c1a-4393-af34-de073a0a8be4</Id>
       <PostTime>2012-05-16T12:40:54.6797054</PostTime>
       <Poster>George_Bush</Poster>
       <Content>Dear Americans, I know that you don't care a lot about me since Obame took my place.</Content>
      </Message>
      <Message>
       <Id>caebcb26-50cb-4125-902f-87b05e50afba</Id>
       <PostTime>2012-05-16T12:41:00.6483026</PostTime>
       <Poster>George_Bush</Poster>
       <Content> But I'm glad I'm still occupying a special place in your mind : the dumb man. With Love. Bushy</Content>
      </Message>
     </Content>
    </Answer>
  
Or, if an error occured, for example you thought the account name was Bush_George :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error Code="AccountNotFound" />
    </Answer>


###Informations

* In **URL**, _accountName_ is the name of the account whose messages you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default value is set to 20.
* In **Response**, _Size_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).


##Read again the recent messages you tagged

###URL

http://api.tigwi.com/account/taggedmessages/_accountName_/_numberOfMessages_
http://api.tigwi.com/account/taggedmessages/name=_accountName_/_numberOfMessages_
http://api.tigwi.com/account/taggedmessages/id=_accountId_/_numberOfMessages_


##See to which accounts someone subscribed

###Purpose

Get a number _numberOfSubscriptions_ of accounts in any of the lists followed by the account _accountName_ or _accountId_. No special order provided.

If you're authenticated and you have the rights on the account, you will see private subscriptions (from private lists) along with public ones.

###HTTP method

GET

###URL
http://api.tigwi.com/account/subscriberaccounts/_accountName_/_numberOfSubscriptions_
http://api.tigwi.com/subscriberaccounts/name=_accountName_/_numberOfSubscriptions_
http://api.tigwi.com/subscriberaccounts/id=_accountId_/_numberOfSubscriptions_


###Response

 
###Informations
* In **URL**, _accountName_ is the name of the account whose subscriptions you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose subscriptions you want to get.
* In **URL**, _numberOfSubscriptions_ is the number of subscriptions you want to get. It is optional and default value is set to 20.
* In **Response**, _Size_ is the number of subscription returned (different from _numberOfSubscriptions_ if there are not enough subscriptions to provide).
* If you're not authorized, you will only receive subscriptions from lists that the owner has declared public.




##See which accounts are following someone

###Purpose

Get a number _numberOfSubscribers_ of accounts that have subscribed a public list in which the account _accountName_ or _accountId_ appears. No special order provided

###HTTP method

GET

###URL

http://api.tigwi.com/account/subscribedaccounts/_accountName_/_numberOfSubscribers_
http://api.tigwi.com/account/subscribedaccounts/name=_accountName_/_numberOfSubscribers_
http://api.tigwi.com/account/subscribedaccounts/id=_accountId_/_numberOfSubscribers_

###Response

 
###Informations
* In **URL**, _accountName_ is the name of the account whose subscribers you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose subscribers you want to get.
* In **URL**, _numberOfSubscribers_ is the number of subscribers you want to get. It is optional and default value is set to 20.
* In **Response**, _Size_ is the number of subscribers returned (different from _numberOfSubscribers_ if there are not enough subscribers to provide).




##See the public lists followed by someone

###Purpose

Get a number _numberOfLists_ of the public lists followed by the account _accountName_ or _accountId_. No particular order provided.

If you're authenticated and you have the rights on the account, you will see private lists along with public ones.

###HTTP method

GET

###URL
http://api.tigwi.com/account/subscribedlists/_accountName_/_numberOfLists_
http://api.tigwi.com/account/subscribedlists/name=_accountName_/_numberOfLists_
http://api.tigwi.com/account/subscribedlists/id=_accountId_/_numberOfLists_

###Response

###Informations
* In **URL**, _accountName_ is the name of the account whose publicly followed lists you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose publicly followed lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _Size_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).
* If you're not authorized, you will only receive lists that the owner has declared public.




##See in which lists someone appears

###Purpose

Obtain a number _numberOfLists_ of public lists where the account _accountName_ or _accountId_ appears. No particular order provided

###HTTP method

GET

###URL
http://api.tigwi.com/account/subscriberlists/_accountName_/_numberOfLists_
http://api.tigwi.com/account/subscriberlists/name=_accountName_/_numberOfLists_
http://api.tigwi.com/account/subscriberlists/id=_accountId_/_numberOfLists_

###Response

###Informations
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default value is set to 20.
* In **Response**, _Size_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).
* There is no way to get private lists who subscribed to an account.

##See someone's owned lists

###Purpose

Obtain a number _numberOfLists_ of the account _accountName_ or _accountId_'s owned lists. No particular order provided.

If you're authenticated and you have the rights on the account, you will see private lists along with public ones.

###HTTP method

GET

###URL
http://api.tigwi.com/account/ownedlists/_accountName_/_numberOfLists_
http://api.tigwi.com/account/ownedlists/name=_accountName_/_numberOfLists_
http://api.tigwi.com/account/ownedlists/id=_accountId_/_numberOfLists_

###Response


###Informations
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default value is set to 20.
* In **Response**, _Size_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).
* If you're not authorized, you will only receive lists that the owner has declared public.


##Get main information about one account

###Purpose

Obtain the account _accountName_ or _acountId_'s main information.

###HTTP method

GET

###URL
http://api.tigwi.com/account/maininfo/_accountName_
http://api.tigwi.com/account/maininfo/name=_accountName_
http://api.tigwi.com/account/maininfo/id=_accountId_

###Response



#Modifying an _account_

**Remember :** since the following ressources use the POST verb, they require authentication.


##Write a message

###HTTP method

POST

###URL

http://api.tigwi.com/account/write

###Request examples


###Response example


###Informations
* You **must** be authenticated as an authorized user of account _nameOfAccount_ to post a message.
* In **Request**, the size of your message is limited to 140 characters and this limit is verified by the server. It raises an error if the message is too long.
* In **Request**, `<AccountName>` is the name of the account where you intend to post a message.
* In **Request**, `<AccountId>` is the unique identifier of the account where you intend to post a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (particularly when they don't refer to the same account).
* In **Response**, the identifier provided is the message created's one.


##Copy a message

###Purpose

To copy a message is to write a message with the same content. You can copy messages sent by others.

Note : Not implemented

###HTTP method

POST

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

###HTTP method

POST

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
* You **must** be authenticated as an authorized user of the account which wrote the message.




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
* It is possible to subscribe a private list just knowing its Guid, even if you're not the owner.

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
