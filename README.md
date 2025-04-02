# Unity Audio Visualiser 🎶
This project initially began as a side project being developed for my sister, who has always enjoyed music with visualiser effects.
working towards the deadline of her birthday, which gave me roughly a month for this project, I had a great time figuring out how to
make use of fast forier transforms and import files in unity for the creation of this program.

The key goals I was working towards with this project were to make it look good (not necessarily complex, just simple and clean), to
make it capable of loading external files in some way, and to make it within the one month deadline. I am pleased to say that all of 
these have been achieved, and I was even able to add some simple visual customisation options before shipping.

## How to Use 📚
The audio visualiser upon being ran will look for a folder in your documents titled "AudioVisualiserMusic". It will look for the audio
you have placed in this folder with the .mp3 file extension, and if the file doesn't exist it will create it. simply put your music 
into this folder and enjoy listening!

## Demo Video ⏯️
<a href="https://www.youtube.com/shorts/TcbWR1ghD74">
  <img src="https://i.ytimg.com/vi/TcbWR1ghD74/oar2.jpg" alt="Watch the video" width="255" height="400">
</a>

## The Development Process 🛠️ 

### 🤔 Defining the Goals
My first step with this project was actually talking to my sister a bit about what she would be interested in. Having these chats and
defining exactly what was required to make the product that she would enjoy was my first time making a project for somebody other than 
myself outside of uni, which was an interesting experience despite obviously being quite unofficial. These experiences were built upon
further through things like my third year professional project, where we worked with industry clients, and I found the lessons learned 
here to come in handy with that project too, espoecially the importance of defining proper goals and end points for the project. 

The first point key point that was brought up was that she wanted to be able to use this with her own music and load in her own songs.
Initially I wanted to do more than just this, as I was hoping to give the program settings to visualise all system audio so that music
didn't necessarily need to be played through the visualiser program, and make it easier than it ended up being to load in files, but this
proved to be a lesson for me in managing scope. I had given myself a month for a project I wasn't working on full time to figure out how
to make systems I had no experience with, and these features had to be cut in the interests of finishing the project for her birthday.

The other point to note was that we wanted it to look nice, since that was the whole point of having the audio visualised in the first place.

### 💡 Starting the Research
For me the next step of this project had to be research. Like many of my other projects, I had no idea what I was doing to begin with. I
looked for resources from people with experience around the internet on how audio visualisers worked, coming across mathematical concepts
like fast forier transforms (a formula for breaking down a given wave or signal into the individual frequencies that make it up and how strong
they are). By applying fast forier transforms to the sound wave our music is generating, audio visualisers will break the sound down into 
frequency bands. Each band will be a different size, often based on how our ears percieve sounds. Generally speaking lower frequencies will
have thinner bands (20-60 hertz for example) whereas higher frequencies will have wider bands (6000 - 20000 hertz), because our ears percieve
these lower frequency sounds with more precision than the higher frequency ones. Breaking down the audio into these bands is important because
we need to break down continuous frequency into single numbers we can use.

Once we have these bands, making the visualiser is just a matter of applying the data on how strongly each band is represented in the audio
to some kind of visual feedback to create nice looking effects, which for me was a fine task since this kind of real time representation
of data made use of a lot of transferable skills of mine from game development (especially since I had chosen to use unity).

For anybody interested in pursuing a similar project, one of the most useful sources for me was (this video series)[https://www.youtube.com/playlist?list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo]
by Peer Play

