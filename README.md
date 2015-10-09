# Fitbit.CSharp

# Overview

Wearables have always intrigued me but scared me at the same time.  Do I really want to track every facet of my daily activity, and then potentially expose it all via an accidental button push that uploads everything that I have eaten for the past year, along with my heart rate data, to facebook?  The answer to that question is obviously no but on the other hand who cares how many packages of peanut m&m’s I eat or how I move during the day?  I am much more than what I eat and how many steps I take.  Or am I?

All fear and worry has been tossed aside to provide the Money20/20 hackathon participants a jumpstart to Fitbit hackathon success.

The Fitbit Surge is a pretty cool product:  GPS, 24 hour wrist based heart rate monitoring, step counter, sleep tracker, milkshake maker.  Ok it does not make milkshakes but stick around for another couple of years and/or attend the Money20/20 hackathon and not only will the device make a milkshake but it will also facilitate paying for the milkshake.  In addition to tracking all of the above information the Fitbit app will allow for additional tracking elements like workouts, food intake, etc. making the Fitbit, and app combo, a very powerful healthy living tool.

The box was cool too, nice presentation (any photo quality issues are mine).

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitpackage.JPG)

Here’s a list of all of the features.

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitpackagefeatures.JPG)

And finally the watch itself!

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitthewatch.JPG)

It's a "super" watch!

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitsuperwatch.JPG)


# Setup

Setup was very easy.  I downloaded the iOS Fitbit app to my iPhone and went through a series of steps.  I think the total time was under one minute and within that minute the scary data feeling came back.  My birthday, gender, height, weight, email address.  In 30 seconds Fitbit knows more about me than my wife.  Granted I could have easily doctored this information but would the device even be usable?  Probably not and I’m doing this for the hackathon participants so there is a higher cause that quashed the scary feeling.  The only downside to this process is that the Fitbit needed to be charged.  Isn’t that the case with all electronics?  The initial excitement killed because the device needs to be charged or did not come with AA batteries.  Oh well, at least the charge seemed quick and the USB to Fitbit cabling nice and neat.

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbittime.JPG)

#Initial Use

I placed the watch on my wrist and immediately it was tracking my steps and heart rate.  Cool stuff.  This was all on a Wednesday, I wore the device over night and was fascinated to see the sleep pattern of someone with two young children.  In the morning the device screamed at me, "you need to learn how to sleep."  I showed it to my son and he was completely unimpressed chanting, "move it, move it."  A reference to the song, "I like to Move it," used throughout the Madagascar series of films.  His sentiment did not fall on deaf ears, when did I become so sedentary?

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitheartrate.JPG)

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitsteps.JPG)

#Heart Rate Tracking

The next test of the heart rate tracking would be a day of work.  What would my heart rate look like when in meetings or what happens before/after eating a bag of peanut m&m’s?  All of this would soon be answered.  Well, maybe and maybe not.  My heart rate certainly fluctuates in meetings but it is difficult to tell exactly what causes the fluctuation.  Is it passion?  Or readjusting in an uncomfortable seat?  It is too early to tell, and there are many confounders, but interesting nonetheless.  Here's a screenshot of the iOS heart rate daily graph.  That spike around 7 or 8pm is likely me attempting to mimic a bear crawling around on the grass.  We all have our favorite "workouts".

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitiosheartrate.PNG)

#Vantiv Challenge

Now that I have all of this data what can we do with it?  That brings us to the Vantiv API challenge.  How can we simplify commerce and/or enable commerce with this type of data?  For this use case we are going to use a recent theme in unused capacity, the sharing economy, and attempt to build a product around shoes.  A shoe sharing product that pays based on the number of steps taken with the shoes.  Hmmmmm…nah, who wants to wear someone else’s smelly shoes?  How about this, a new business model for selling shoes?  Instead of purchasing shoes outright we’re going to “rent” shoes to consumers and charge a fee based on the number of steps they take as long as they provide us access to their daily Fitbit data and as long as they take at least 5000 steps a day.  Now being hackers we are already thinking about how to game this proposal but let’s run with it and see what happens.

How would we go about building something like this and integrating to the Vantiv API.  The perfect solution is a website that captures the user’s initial information:  Fitbit account, billing/shipping info, shoe size, one or two selected models, and credit card information.  We will then use Vantiv’s API via Hosted Checkout to perform a ZeroAuth to verify the card and receive a token which we will then use for subsequent monthly billing transactions.  The total cost of the monthly fee will be determined based on accessing the user’s Fitbit data via the Fitbit API.  You can browse the API here:  https://dev.Fitbit.com/.  You will need to sign up for the Fitbit API and register an application.

The data collection portion and initial auth, in this case a ZeroAuth which means we do not charge the card we simply validate that it is a real card, is fairly simple.  Navigate over to:  https://github.com/MercuryPay/HostedCheckout.MVC.CSharp where you will find a c# integration to our Hosted Checkout API.  Using this template we can easily capture the token we will need to store for later use and tweak the template to save the other information suggested above.  We're not going to actually implement that logic as it should be fairly straightforward.  In addition to the c# example feel free to browse around on the mercurypay GitHub site where you will find PHP, vb6, and Objective C integrations to the Hosted Checkout API.

Assuming we have a token, representing the customer's credit card, and some personal information it's time to charge the monthly rental fee based on steps taken so let's determine how to get the number of steps taken per month from a user's Fitbit account.

To get things started register your application at Fitbit.  Here you include various information about your application and type of authentication method you will use.  I decided to use OAuth 2.0 server authentication which allows the user to provide this invoicing app permission to access his/her data, the Fitbit API then returns a code that I use to retrieve an access and refresh token.  Finally whenever the app makes an API call to Fitbit it passes the access token along to authorize.

The authorization portion was the most difficult to implement however, there are OAuth 2.0 libraries floating around that might make this easier.  You can see the implementation in the code.  As with the Raspberry Pi repo the code is pretty ugly but in a state that it should be fairly easy to follow.

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitinvoicer.PNG)

* Website launches
* User clicks the Authentication link
* User provides authorization by logging into their Fitbit account and granting access
* Fitbit redirects back to the URL configured when registering the application and passes a code in query string
* Use that code and client secret to request an access/refresh token for the user
* Use that access token when making Fitbit API calls
* Make the API call to return total number of steps for one month and calculate invoice amount based on one penny per step.
* Call the Vantiv API to process the charge using the token received from the initial Hosted Checkout integration.

![Fitbit.CSharp](https://github.com/mercurypay/Fitbit.CSharp/blob/master/images/fitbitinvoicerstepstaken.PNG)

Eeeps!  Those are some expensive shoes!  Probably should bill for this by flipping the billing around.  We want to incentivize people to use our shoes, and be healthy, the more you use the shoes the less you pay.  The angle we're working here is to obtain data and then we're going to use that data to provide additional value added services to our customers.  Give away the shoes for something more valuable.

#Nice to Know

* The web.config contains configuration info used per application.  Basically I did not want to push my app info to GitHub although the Fitbit API allows an easy way to regenerate client key and secret...just in case.

* The JavaScriptSerializer and the dynamic keyword make for quick JSON parsing, but there are also great JSON libraries out there as well.  There's something about using the dynamic keyword though that makes development feel magical.

* Obtaining the steps data was a little odd, instead of using the date I provided and then returning one month in the future from that date the API returned one month previous to the date.

* While the dynamic keyword is fun it might be more useful to deserialize the data to a real class model.

* I did not consider at least 5000 steps per day or other random things.  The goal of this exercise was to make sure everything would work and to find any large gotchas.  So far the Fitbit/commerce integration feels pretty decent.

* While this app does everything in one step more than likely a real app would retrieve the access/refresh token after validating the customer's credit card and then store them for later use.  "Later use" means when invoicing all we would need is to use the refresh token to generate a new access token and then we could get the steps for that month. Beautiful!
