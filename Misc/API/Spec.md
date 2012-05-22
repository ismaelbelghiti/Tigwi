Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#General presentation of the Tigwi API

##What you can do with our API

If you're a smartphone developper, you may be willing to make your own application to access Tigwi from everywhere. The simpliest way to do that is to use our API. Your software will be able to join our servers to get recent sent messages, see who's following who and even to post some content.

Our API provides a lot of functions. You can even create new lists, follow new people by adding them to one of your lists, follow public lists made by a complete stranger or tag messages you like the most.

If you're a webmaster, you would like to show your last posts ? You can use our API to make a rather simple javascript application doing that.

##Note about answers and errors

All answers are wrapped in the XML root `<Answer> </Answer>` whose sons are `<Content> </Content>` and `<Error\>`. Only one of them will appear. In the case where no error has occured but no content is expected in the answer, you will receive an empty _Error_ tag :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>

When an error occured you will get a description of it in the attribute _Code_ :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error Code="Subscription missing" />
    </Answer>

Finally, if you have a successful request expecting a real answer you could have something like :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="NewObject" Id="312e2061-3a79-4f82-a53b-e77af1ff0e59" />
    </Answer>

or

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Lists" Size="1">
      <List>
       <Id>312e2061-3a79-4f82-a53b-e77af1ff0e59</Id>
       <Name>Presidents of the US</Name>
      </List>
     </Content>
    </Answer>

##Note about authentication

Some of our ressources require authentication. This is the case if you use any with a POST verb but also if you want to access to private data such as an user email, or an account private lists.

To authenticate, you need to send an HTTP cookie named _key_ whose value should be the user login for the moment, along with the request.
This means that if you're writing requests manually you should write something like this :

    Cookie: key=John_Smith_login

If authentication went wrong, you would have a code of error among :

* "No key cookie was sent"
* "Authentication failed" (if the key does not match any user)
* "User hasn't rights on this account"

##Note about identifying an account

For every ressource depending on an account, you can give its name or its unique identifier. For GET methods, the address isn't exactly the same ; for POST methods, you have two tags, `<AccountName>` and `<AccountId>`, and you must choose one of them. Access through unique identifier is more direct. Thus, if you fill both `<AccountName>` and `<AccountId>` tags, only `<AccountId>` will be taken into consideration.


#Get information about an _account_

##Read last messages wrote by someone

###Purpose

Get the number you want to of an account last sent messages.

###HTTP method

GET

###URL

http://api.tigwi.com/account/messages/_accountName_/_numberOfMessages_

http://api.tigwi.com/account/messages/name=_accountName_/_numberOfMessages_

http://api.tigwi.com/account/messages/id=_accountId_/_numberOfMessages_

###Response

Example of a response if you requested for 
http://api.tigwi.com/account/messages/John_Smith/30

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Messages" Size="2">
      <Message>
       <Id>1ccacf01-d531-4062-930a-7a2a9fad9559</Id>
       <PostTime>2012-05-16T12:37:23.9819744</PostTime>
       <Poster>John_Smith</Poster>
       <Content>Tigwi is great ! This is my first message. I've just joined. I encourage you to do so.</Content>
      </Message>
      <Message>
       <Id>d89814c5-b61f-4820-b8c0-49fb179649f8</Id>
       <PostTime>2012-05-16T12:38:13.0275938</PostTime>
       <Poster>John_Smith</Poster>
       <Content>Well, there doesn't seem to be a lot of people that heard of my last advice.</Content>
      </Message>
     </Content>
    </Answer>
  
Or, if an error occured, for example you thought the account name was Smith_John :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error Code="AccountNotFound" />
    </Answer>


###Information

* In **URL**, _accountName_ is the name of the account whose messages you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default value is set to 20.
* In **Response**, _Size_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).


##Read again the recent messages you tagged

###Purpose

Get a number _numberOfMessages_ of messages from your favourites. Authentication required.

###HTTP method

GET

###URL

http://api.tigwi.com/account/taggedmessages/_accountName_/_numberOfMessages_

http://api.tigwi.com/account/taggedmessages/name=_accountName_/_numberOfMessages_

http://api.tigwi.com/account/taggedmessages/id=_accountId_/_numberOfMessages_

###Response

Example of a response if you requested for 
http://api.tigwi.com/account/taggedmessages/John_Smith/30

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Messages" Size="2">
      <Message>
       <Id>1ccacf01-d531-4062-930a-7a2a9fad9559</Id>
       <PostTime>2012-06-16T12:37:23.9819744</PostTime>
       <Poster>Paul_Smith</Poster>
       <Content>I'm pleased to find out that my brother thinks Tigwi is great too.</Content>
      </Message>
      <Message>
       <Id>d89814c5-b61f-4820-b8c0-49fb179649f8</Id>
       <PostTime>2012-07-16T12:38:13.0275938</PostTime>
       <Poster>Global_daily</Poster>
       <Content>Hit news : more than 200,000,000 users on Tigwi !</Content>
      </Message>
     </Content>
    </Answer>

###Information

* In **URL**, _accountName_ is the name of the account whose favourites messages you want to get.
* In **URL**, _accountId_ is the unique identifier of the account whose favourites messages you want to get.
* You **must** be authenticated as an authorized user of the account to see the tagged messages.


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


###Response example


 
###Information
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

###Response example



##See the lists followed by someone

###Purpose

Get a number _numberOfLists_ of the public lists followed by the account _accountName_ or _accountId_. No particular order provided.

If you're authenticated and you have the rights on the account, you will see private lists along with public ones.

###HTTP method

GET

###URL
http://api.tigwi.com/account/subscribedlists/_accountName_/_numberOfLists_

http://api.tigwi.com/account/subscribedlists/name=_accountName_/_numberOfLists_

http://api.tigwi.com/account/subscribedlists/id=_accountId_/_numberOfLists_

###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Lists" Size="2">
      <List>
       <Id>312e2061-3a79-4f82-a53b-e77af1ff0e59</Id>
       <Name>Family</Name>
      </List>
      <List>
       <Id>d89814c5-b61f-4820-b8c0-49fb179649f8</Id>
       <Name>Work at Tigwi</Name>
      </List>
     </Content>
    </Answer>

###Information
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

###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Lists" Size="2">
      <List>
       <Id>312e2061-3a79-4f82-a53b-e77af1ff0e59</Id>
       <Name>Family</Name>
      </List>
      <List>
       <Id>d89814c5-b61f-4820-b8c0-49fb179649f8</Id>
       <Name>Work at Tigwi</Name>
      </List>
     </Content>
    </Answer>

###Information
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

###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="Lists" Size="1">
      <List>
       <Id>312e2061-3a79-4f82-a53b-e77af1ff0e59</Id>
       <Name>Presidents of the US</Name>
      </List>
     </Content>
    </Answer>

###Information

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

###Response example



#Modifying an _account_

**Remember :** since the following ressources use the POST verb, they require authentication.


##Write a message

###HTTP method

POST

###URL

http://api.tigwi.com/account/write

###Request example

    <Write>
     <AccountName>John_Smith</AccountName>
     <Message>I love Tigwi</Message>
    </Write>

or

    <Write>
     <AccountId>312e2061-3a79-4f82-a53b-e77af1ff0e59</AccountId>
     <Message>I love Tigwi</Message>
    </Write>

###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="ObjectCreated" Id="7a2a8d74-621a-455c-bfc3-8552474cc735" />
    </Answer>

###Information
* In **Request**, the size of your message is limited to 140 characters and this limit is verified by the server. It raises an error if the message is too long.
* In **Request**, `<AccountName>` is the name of the account where you intend to post a message.
* In **Request**, `<AccountId>` is the unique identifier of the account where you intend to post a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (particularly when they don't refer to the same account).
* You **must** be authenticated as an authorized user of the account to post a message.
* In **Response**, the unique identifier is the one of the new message.


##Copy a message

###Purpose

To copy a message is to write a message with the same content. You can copy messages sent by others.

Note : Not implemented

###HTTP method

POST

###URL

http://api.tigwi.com/account/copy

###Request examples



###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="ObjectCreated" Id="7a2a8d74-621a-455c-bfc3-8552474cc735" />
    </Answer>

###Information
* In **Request**, `<AccountName>` is the name of the account where you intend to copy a message.
* In **Request**, `<AccountId>` is the unique identifier of the account where you intend to copy a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).
* You **must** be authenticated as an authorized user of the account to copy a message.


##Remove a message

Note : not implemented

###HTTP method

POST

###URL

http://api.tigwi.com/account/delete

###Request example



###Response

If everything went well :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>

###Information

* You **must** be authenticated as an authorized user of the account which wrote the message.


##Add a message to an account's favourite

###Purpose

To tag a message as one of your favourites. Authentication required.

###HTTP method

POST

###URL

http://api.tigwi.com/account/tag

###Request examples

    <Tag>
        <AccountName>John_Smith</AccountName>
        <MessageId>f41d6ebf-4e50-48bc-acdb-ab24359455fc</MessageId>
    </Tag>

or

    <Tag>
        <AccountId>312e2061-3a79-4f82-a53b-e77af1ff0e59</AccountId>
        <MessageId>f41d6ebf-4e50-48bc-acdb-ab24359455fc</MessageId>
    </Tag>

###Response

If everything went well :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>

###Information

* In **Request**, `<AccountName>` is the name of the account where you intend to tag a message.
* In **Request**, `<AccountId>` is the unique identifier of the account where you intend to tag a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).
* You **must** be authenticated as an authorized user of the account where you intend to tag a message.


##Remove a message from an account's favourite

###Purpose

To remove a message from your favourites. Authentication required.

###HTTP method

POST

###URL

http://api.tigwi.com/account/untag

###Request

    <Untag>
        <AccountName>John_Smith</AccountName>
        <MessageId>f41d6ebf-4e50-48bc-acdb-ab24359455fc</MessageId>
    </Untag>

or

    <Untag>
        <AccountId>312e2061-3a79-4f82-a53b-e77af1ff0e59</AccountId>
        <MessageId>f41d6ebf-4e50-48bc-acdb-ab24359455fc</MessageId>
    </Untag>

###Response

If everything went well :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>

###Information

* In **Request**, `<AccountName>` is the name of the account where you intend to untag a message.
* In **Request**, `<AccountId>` is the unique identifier of the account where you intend to untag a message.
* In **Request**, if you use both `<AccountName>` and `<AccountId>`, only the `<AccountId>` will be used (in particular when they don't refer to the same account).
* You **must** be authenticated as an authorized user of the account where you intend to untag a message.


##Create a list

###Purpose

If you wish to follow people, you must before create a new, empty list.
Authentication required.

###HTTP method

POST

###URL

http://api.tigwi.com/account/createlist/

###Request example

	<CreateList>
     <AccountName>John_Smith</AccountName>
     <ListInfo>
      <Name>Presidents of the US</Name>
      <Description>To keep touch</Description>
     </ListInfo>
    </CreateList>
or

	<CreateList>
     <AccountId>312e2061-3a79-4f82-a53b-e77af1ff0e59</AccountId>
     <ListInfo>
      <Name>Presidents of the US</Name>
      <Description>To keep touch</Description>
     </ListInfo>
    </CreateList>

###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="ObjectCreated" Id="312e2061-3a79-4f82-a53b-e77af1ff0e59" />
    </Answer>

###Information

* You **must** be authenticated and authorized to use the account.
* In **Request**, `<AccountName>` is the name of the account who wants to create the list _nameOfList_.
* In **Request**, `<AccountId>` is the unique identifier of the account who wants to create the list _nameOfList_.
* In **Request**, `<Name>` is the name you want to give to the new list.
* In **Request**, `<Description>` is a short text to remember what the list is about.
* In **Request**, _privateSetting_ value must be _false_ if you want the new list to be public or _true_ if only you can see that list.


##Make an account subscribe to a list

###Purpose

For someone to distantly subscribe to a list. Authentication required.

###HTTP method

POST

###URL

http://api.tigwi.com/account/subscribelist/

###Request
    
    <SubscribeList>
        <AccountName>John_Smith</AccountName>
        <Subscription>312e2061-3a79-4f82-a53b-e77af1ff0e59</Suscription>
    </SubscribeList>

or

    <SubscribeList>
        <AccountId></AccountId>
        <Subscription>312e2061-3a79-4f82-a53b-e77af1ff0e59</Suscription>
    </SubscribeList>

###Response

If everything went well :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>

###Information
* You **must** be authenticated as an authorized user of account _nameOfSubscriber_ to use this method.
* In **Request**, `<AccountName>` is the name of the account who wants to follow the list whose unique identifier is `<Subscription>`.
* In **Request**, `<AccountId>` is the unique identifier of the account who wants to follow the list whose unique identifier is `<Subscription>`.
* It is possible to subscribe a private list just knowing its unique identifier, even if you're not the owner.


#Get information about a _List_

##Get accounts followed by the list

###Purpose

Obtain a number _numberOfSubscriptions_ of accounts followed by list whose unique identifier is _idOfList_.
No particular order provided

###HTTP method

GET

###URL

http://api.tigwi.com/list/subscriptions/_idOfList_/_numberOfSubscriptions_

###Response



###Information

* In **URL**, _idOfList_ is the id of the list whose information you want to get
* In **URL**, _numberOfSubscriptions_ is the number of accounts you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of accounts returned (different from _numberOfSubscriptions_ if there are not enough accounts to provide).


##Get the list of accounts following a given list

###Purpose

Obtain a number _numberOfFollowers_ of accounts following the given list whose unique identifier is _idOfList_.
No particular order provided

###HTTP method

GET

###URL

http://api.tigwi.com/list/subscribers/_idOfList_/_numberOfFollowers_

###Response



###Information
* In **URL** _idOfList_ is the unique identifier of the list whose followers you want to get.
* In **URL**, _numberOfFollowers_ is the number of accounts you want to get. It is optional and default is set to 20.
* In **Response**, _Size_ is the number of accounts returned (different from _numberOfFollowers_ if there are not enough accounts to provide).


##Get a list's owner information

###Purpose

Obtain information about the owner of list whose unique identifier is _idOfList_.

###HTTP method

GET

###URL

http://api.tigwi.com/list/owner/_idOfList_

###Response



###Information

* In **URL**, _idOfList_ is the unique identifier of the list whose owner you want to get.


##Get last messages sent to a list

###Purpose

Obtain a number _numberOfMessages_ of last messages sent to the list whose unique identifier is _idOfList_.

###HTTP method

GET

###URL

http://api.tigwi.com/list/messages/_idOfList_/_numberOfMessages_

###Response



###Information

* In **URL**, _idOfList_ is the unique identifier of the list whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _Size_ is the number of messages returned (different from _numberOfMessages_ if there are not enough accounts to provide).


#Modifying a _list_

**Remember :** since the following ressources use the POST verb, they require authentication. You must be authenticated as an user with appropriate autorization on the list you want to modify.


##Add an account to a list

###HTTP method

POST

###URL

http://api.tigwi.com/list/subscribeaccount/

###Request example
    
    <ListAndAccount>
        <List>312e2061-3a79-4f82-a53b-e77af1ff0e59</List>
        <Account>f41d6ebf-4e50-48bc-acdb-ab24359455fc</Account>
    </ListAndAccount>

###Response

If everything went well :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>
	
###Information

* You **must** be authenticated and authorized to use the owner of the list whose unique identifier is given by `<List>` to use this method.
* In **Request**, `<List>` is the unique identifier of the list who wants to follow the account `<Account>`.


##Delete an account from a list

###HTTP method

POST

###URL

http://api.tigwi.com/list/unsubscribeaccount/

###Request example
    
    <ListAndAccount>
        <List>312e2061-3a79-4f82-a53b-e77af1ff0e59</List>
        <Account>f41d6ebf-4e50-48bc-acdb-ab24359455fc</Account>
    </ListAndAccount>

###Response

If everything went well :

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Error />
    </Answer>

###Information

* You **must** be authenticated and authorized to use the owner of the list whose unique identifier is given by `<List>` to use this method.
* In **Request**, `<List>` is the unique identifier of the list who wants to stop following the account `<Account>`.


#Get information about an _user_

###Purpose

Obtain main information of yourself as an user. Authentication required.

###HTTP method

GET

###URL

http://api.tigwi.com/user/maininfo/

###Response example

    <?xml version="1.0"?>
    <Answer xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
     <Content xsi:type="User">
      <Login>John_Smith</Login>
      <Email>falseaddress@yohoomail.com</Email>
      <Id>f41d6ebf-4e50-48bc-acdb-ab24359455fc</Id>
     </Content>
    </Answer>

###Information

You don't need to give any detail on the user you want to have the information because you are identified through authentication.