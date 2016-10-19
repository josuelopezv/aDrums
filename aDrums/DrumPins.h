// DrumPins.h

#ifndef _DRUMPINS_h
#define _DRUMPINS_h

//#if defined(ARDUINO) && ARDUINO >= 100
//	#include "arduino.h"
//#else
//	#include "WProgram.h"
//#endif

#define MAX_PIN_SIZE 37

//pin types
#define PINTYPE_DISABLED 0
#define PINTYPE_PIEZO 1	
#define PINTYPE_TRIMPOT 2
#define PINTYPE_SWITCH 3

#define ADCD1 PIND1
#define ADCD2 PIND2
#define ADCD3 PIND3

#define FLAG_IC1 1
#define FLAG_IC2 2
#define FLAG_IC3 4

#define getPinLoc(pin,IC_Pin) (((pin) << 3) | (IC_Pin))



#endif

