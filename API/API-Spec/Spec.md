Tigwi - API Specification by L. de HARO, A. de MYTTENAERE and T. ZIMMERMANN 

#General methods

##Create an user
###Purpose
To create an user.
###HTTP method
*POST*
###URL
http://api.tigwi.com/api/createuser
###Request

    <User>
	   <Login> userLogin </Login>
       <Email> <!-- user's email address--> </Email>
       <Password> <!-- First user's password --> </Password>
    </User>

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

