Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get informations about an _account_

##Get someone's recently sent messages
###Purpose
Obtain a number _n_ of the account _accountname_ 's last posted messages
###HTTP method
*GET*
###URL
http://api.tigwi.com/accountmessages/accountname/numberOfMessages
###Request
_left empty_
###Response

    <MessageList Size="sizeOfList">
	    <Message> <!-- See below --> </Message>
	    <Message> ... </Message>
	    ...
	    <Message> ... </Message>
    </MessageList>

Message format:

     <Message>
	     <Id> idOfMessage </Id>
	     <PostTime> timeOfPost </PostTime>
	     <Poster> nameOfUser </Poster>
	     <Content> content </Content>
     </Message>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _accountname_ is the name of the account whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).

##Get someone's subscriptions list
###Purpose
Obtain a number _n_ of the account _accountname_ 's subscriptions. 
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/accountsubscriptions/accountname/numberOfSubscriptions
###Request
_left empty_
###Response

    <AccountsList Size="sizeOfList">
	    <Account> <!-- See below --> </Account>
	    <Acount> ... </Account>
	    ...
	    <Account> ... </Account>
    </AccountsList>

Account format:

     <Account>
	     <Id> idOfAccount </Id>
	     <Name> nameOfAccount </Name>
     </Account>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _accountname_ is the name of the account whose subscriptions you want to get.
* In **URL**, _numberOfSubscriptions_ is the number of subscriptions you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of subscription returned (different from _numberOfSubscriptions_ if there are not enough subscriptions to provide).


##Get someone's subscribers list
###Purpose
Obtain a number _n_ of the account _accountname_ 's subscribers.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/accountsubscribers/name/numberOfSubscribers
###Request
_left empty_
###Response

    <AccountsList Size="sizeOfList">
	    <Account> <!-- See below --> </Account>
	    <Account> ... </Account>
	    ...
	    <Account> ... </Account>
    </AccountsList>

Account format:

     <Account>
	     <Id> idOfAccount </Id>
	     <Name> nameOfAccount </Name>
     </Account>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _accountname_ is the name of the account whose subscribers you want to get.
* In **URL**, _numberOfSubscribers_ is the number of subscribers you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of subscribers returned (different from _numberOfSubscribers_ if there are not enough subscribers to provide).

##Get an account's followed public lists
###Purpose
Obtain a number _n_ of the account _accountName_ 's followed public lists.
No particular order provided.
###HTTP method
*GET*
###URL
http://api.tigwi.com/accountsubscribedpubliclists/accountName/numberOfLists
###Request
_left empty_
###Response

    <ListsList Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </ListsList>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

In case an error occurs:

    <Error Number="codeOfError"/>

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
http://api.tigwi.com/accountsubscribedlists/accountName/numberOfLists
###Request
_left empty_
###Response

    <ListsList Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </ListsList>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _accountName_ is the name of the account whose followed lists you want to get.
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).

##Get an account's owned public lists
###Purpose
Obtain a number _n_ of the account _accountName_ 's owned public lists.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/accountpubliclists/accountName/numberOfLists
###Request
_left empty_
###Response

    <ListsList Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </ListsList>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _accountName_ is the name of the account whose lists you want to get.
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
http://api.tigwi.com/accountlists/accountName/numberOfLists
###Request
_left empty_
###Response

    <ListsList Size="sizeOfList">
	    <List> <!-- See below --> </List>
	    <List> ... </List>
	    ...
	    <List> ... </List>
    </ListsList>

List format:

     <List>
	     <Id> idOfList </Id>
	     <Name> nameOfList </Name>
     </List>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* You **must** be authenticated as _accountName_ to have access to these informations. 
* In **URL**, _numberOfLists_ is the number of lists you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of lists returned (different from _numberOfLists_ if there are not enough lists to provide).



#Get informations about a _List_
##Get accounts followed by the list
###Purpose
Obtain a number _n_ of accounts followed by list with id _idOfList_.
No particular order provided
###HTTP method
*GET*
###URL
http://api.tigwi.com/listsubscriptions/idOfList/numberOfSubscriptions
###Request
_left empty_
###Response

    <AccountsList Size="sizeOfList">
	    <Account> <!-- See below --> </Account>
	    <Account> ... </Account>
	    ...
	    <Account> ... </Account>
    </AccountsList>

Account format:

     <Account>
	     <Id> idOfAccount </Id>
	     <Name> nameOfAccount </Name>
     </Account>

In case an error occurs:

    <Error Number="codeOfError"/>

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
http://api.tigwi.com/listsubscribers/idOfList/numberOfFollowers
###Request
_left empty_
###Response

    <AccountsList Size="sizeOfList">
	    <Account> <!-- See below --> </Account>
	    <Account> ... </Account>
	    ...
	    <Account> ... </Account>
    </AccountsList>

Account format:

     <Account>
	     <Id> idOfAccount </Id>
	     <Name> nameOfAccount </Name>
     </Account>

In case an error occurs:

    <Error Number="codeOfError"/>

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
http://api.tigwi.com/listowner/idOfList
###Request
_left empty_
###Response

     <Account>
	     <Id> idOfAccount </Id>
	     <Name> nameOfAccount </Name>
     </Account>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _idOfList_ is the id of the list whose owner you want to get.

##Get last messages sent to a list
###Purpose
Obtain a number _n_ of last messages sent to the list with id _idOfList_.
###HTTP method
*GET*
###URL
http://api.tigwi.com/listtimeline/idOfList/numberOfMessages
###Request
_left empty_
###Response

    <MessageList Size="sizeOfList">
	    <Message> <!-- See below --> </Message>
	    <Message> ... </Message>
	    ...
	    <Message> ... </Message>
    </MessageList>

Message format:

     <Message>
	     <Id> idOfMessage </Id>
	     <PostTime> timeOfPost </PostTime>
	     <Poster> nameOfUser </Poster>
	     <Content> content </Content>
     </Message>

In case an error occurs:

    <Error Number="codeOfError"/>

###Informations
* In **URL**, _idOfList_ is the id of the list whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough accounts to provide).


#Send informations to one's Tigwi account

This methods require authentication. You must be authenticated as a _user_ with permissions to use the _account_ you want to modify.

##Post a message
###Purpose
For someone to send a message on an authorized account. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/write
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

    <Error Number="codeOfError"/>


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
http://api.tigwi.com/createlist/

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

    <Error Number="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated and authorized to use _nameOfSubscriber_ to use this method.

* In **Request**, _nameOfSubscriber_ is the name of the account who wants to follow the list _nameOfSubscription_.

* In **Request**, _nameOfList_ is the name you want to give to the new list.

* In **Request**, _aRapidDescription_ is a short text to remember what the list is about.

* In **Request**, _privateSetting_ value must be _false_ if you want the new list to be public or _true_ if only you can see that list.

##Subscribe to a list
###Purpose
For someone to distantly subscribe to a list. Authentication required.
###HTTP method
*POST*
###URL
http://api.tigwi.com/accountsubscribelist/
###Request
    
    <Subscribe>
        <Account> nameOfSubscriber </Account>
        <Subscription> idOfSubscription </Suscription>
    </Subscribe>

###Response
In case an error occurs

    <Error Number="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated and authorized to use the account _nameOfSubscriber_ to use this method.
* In **Request**, _nameOfSubscriber_ is the name of the account who wants to follow the list _idOfSubscription_.

#Modifying a list
##Suscribe to an account
###Purpose
For a list to add a suscription to a given account. Authentication required.

###HTTP method
*POST*
###URL
http://api.tigwi.com/listsubscribeaccount/
###Request
    
    <ListSubscribe>
        <List> idOfSuscriber </List>
        <Subscription> nameOfSubscription </Suscription>
    </ListSubscribe>

###Response
In case an error occurs

    <Error Number="codeOfError"/>

If no error occurs

    <Error/>


###Informations
* You **must** be authenticated and authorized to use the owner of the list with id _idOfSuscriber_ to use this method.
* In **Request**, _idOfSuscriber_ is the id of the list who wants to follow the account _nameOfSubscription_.