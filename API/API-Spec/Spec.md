Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get an user's public informations

##Get someone's messages
###Purpose
Obtain a number _n_ of the user _name_ 's last posted messages
###HTTP method
*GET*
###URL
http://api.tigwi.com/usertimeline/name/numberOfMessages
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

###Informations
* In **URL**, _name_ is the name of the user whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).

##Get someone's subscriptions list
###Purpose
Obtain a number _n_ of the user _name_ 's last subscriptions
###HTTP method
*GET*
###URL
http://api.tigwi.com/usersubscriptions/name/numberOfSubscriptions
###Request
_left empty_
###Response

    <UserList Size="sizeOfList">
	    <User> <!-- See below --> </User>
	    <User> ... </User>
	    ...
	    <User> ... </User>
    </UserList>

User format:

     <User>
	     <Id> idOfUser </Id>
	     <Name> name </Name>
     </User>

###Informations
* In **URL**, _name_ is the name of the user whose subscriptions you want to get.
* In **URL**, _numberOfSubscriptions_ is the number of subscriptions you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of subscription returned (different from _numberOfSubscriptions_ if there are not enough subscriptions to provide).


##Get someone's subscribers list
###Purpose
Obtain a number _n_ of the user _name_ 's last subscribers
###HTTP method
*GET*
###URL
http://api.tigwi.com/usersubscribers/name/numberOfSubscribers
###Request
_left empty_
###Response

    <UserList Size="sizeOfList">
	    <User> <!-- See below --> </User>
	    <User> ... </User>
	    ...
	    <User> ... </User>
    </UserList>

User format:

     <User>
	     <Id> idOfUser </Id>
	     <Name> name </Name>
     </User>

###Informations
* In **URL**, _name_ is the name of the user whose subscribers you want to get.
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
       <User> name <User> 
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


##Subscribe to someone's  channel
###Purpose
For someone to distantly subscribe to another user's channel.
###HTTP method
*POST*
###URL
http://api.tigwi.com/subscribe/
###Request
    
    <Subscribe>
        <User> nameOfSubscriber </User>
        <Subscription> nameOfSubscription </Suscription>
    </Subscribe>

###Response
In case an error occurs

    <Error Number="codeOfError"/>

If no error occurs

    <Error/>


###Informations
