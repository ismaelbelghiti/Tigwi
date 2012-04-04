Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get an user's public informations

##Get someone's messages
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
Obtain a number _n_ of the account _accountname_ 's subscriptions
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

User format:

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
Obtain a number _n_ of the account _accountname_ 's subscribers
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

User format:

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


#Send informations to one's Tigwi account

##Post a message
###Purpose
For someone to send a message on his own account
###HTTP method
*POST*
###URL
http://api.tigwi.com/write
###Request

    <Write>
       <Account> name <Account> 
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
* In **Request**, the size of your message is limited to 140 characters, but this limit is tested by the server. It raises an error if the message is too long.


##Subscribe to a list
###Purpose
For someone to distantly subscribe to another user's channel.
###HTTP method
*POST*
###URL
http://api.tigwi.com/subscribelist/
###Request
    
    <Subscribe>
        <Account> nameOfSubscriber </Account>
        <Subscription> nameOfSubscription </Suscription>
    </Subscribe>

###Response
In case an error occurs

    <Error Number="codeOfError"/>

If no error occurs

    <Error/>


###Informations
