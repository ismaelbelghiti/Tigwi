Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#Get an user's public informations

##Get someone's messages
###Objective
Obtain a number _n_ of the user _name_ 's last posted messages
###HTTP method
*GET*
###URL
http://api.tigwi.com/usertimeline/_name_/_numberOfMessages_
###Request
_left empty_
###Response

    <MessageList size="sizeOfList">
	    <Message> <!-- See below --> </Message>
	    <Message> ... </Message>
	    ...
	    <Message> ... </Message>
    </MessageList>

Message format:

     <Message>
	     <id> idOfMessage </id>
	     <post_time> timeOfPost </post_time>
	     <poster> nameOfUser </poster>
	     <content> content </content>
     </Message>

###Informations
* In **URL**, _name_ is the name of the user whose messages you want to get.
* In **URL**, _numberOfMessages_ is the number of messages you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of messages returned (different from _numberOfMessages_ if there are not enough messages to provide).

##Get someone's suscriptions list
###Objective
Obtain a number _n_ of the user _name_ 's last suscriptions
###HTTP method
*GET*
###URL
http://api.tigwi.com/usersuscription/_name_/_numberOfSuscriptions_
###Request
_left empty_
###Response

    <UserList size="sizeOfList">
	    <User> <!-- See below --> </User>
	    <User> ... </User>
	    ...
	    <User> ... </User>
    </UserList>

User format:

     <User>
	     <id> idOfUser </id>
	     <name> name </name>
	     <poster> nameOfUser </poster>
	     <content> content </content>
     </User>

###Informations
* In **URL**, _name_ is the name of the user whose suscriptions you want to get.
* In **URL**, _numberOfSuscriptions_ is the number of suscriptions you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of suscription returned (different from _numberOfSuscriptions_ if there are not enough suscriptions to provide).


##Get someone's suscribers list
###Objective
Obtain a number _n_ of the user _name_ 's last suscribers
###HTTP method
*GET*
###URL
http://api.tigwi.com/usersuscribers/_name_/_numberOfSuscribers_
###Request
_left empty_
###Response

    <UserList size="sizeOfList">
	    <User> <!-- See below --> </User>
	    <User> ... </User>
	    ...
	    <User> ... </User>
    </UserList>

User format:

     <User>
	     <id> idOfUser </id>
	     <name> name </name>
	     <poster> nameOfUser </poster>
	     <content> content </content>
     </User>

###Informations
* In **URL**, _name_ is the name of the user whose suscribers you want to get.
* In **URL**, _numberOfSuscribers_ is the number of suscribers you want to get. It is optional and default is set to 20.
* In **Response**, _sizeOfList_ is the number of suscribers returned (different from _numberOfSuscribers_ if there are not enough suscribers to provide).


#Send informations to one's Tigwi account

##Post a message
###Objective
For someone to send a message on his own account
###HTTP method
*POST*
###URL
http://api.tigwi.com/write
###Request

    <Write>
       <User> name <User> 
       <Message>
            <content> <!-- your message --> </content>
       </Message>
    </write>

###Response
In case an error occurs

    <error number="codeOfError">


If no error occurs

    <error/>

###Informations
* In **Request**, the size of your message is limited to 140 characters, but this limit is tested by the server. It raises an error if the message is too long.


##Suscribe to someone's  channel
###Objective
For someone to distantly suscribe to another user's channel.
###HTTP method
*POST*
###URL
http://api.tigwi.com/suscribe/
###Request
    
    <Suscribe>
        <User> nameOfSuscriber </User>
        <Suscription> nameOfSuscription </Suscription>
    </Suscribe>

###Response
In case an error occurs

    <error number="codeOfError">

If no error occurs

    <error/>


###Informations
