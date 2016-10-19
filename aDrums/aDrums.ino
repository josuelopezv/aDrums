#include "DrumPins.h"
#include <MIDIUSB.H>
#include <EEPROM.h>

#define VersionMajor 1
#define VersionMinor 0

#pragma region arrays
byte pinLocation[MAX_PIN_SIZE] = {
	getPinLoc(A6	,	0),	//	HiHat Pedal
	getPinLoc(A7	,	0),	//	HiHat Cymbal T
	getPinLoc(A8	,	0),	//	HiHat Cymbal R
	getPinLoc(A9	,	0),	//	Kick Drum
	getPinLoc(A0	,	0),	//	Snare T
	getPinLoc(A0	,	1),	//	Snare R
	getPinLoc(A0	,	2),	//	Tom 1 T
	getPinLoc(A0	,	3),	//	Tom 1 R
	getPinLoc(A0	,	4),	//	Tom 2 T
	getPinLoc(A0	,	5),	//	Tom 2 R
	getPinLoc(A0	,	6),	//	Tom 3 T
	getPinLoc(A0	,	7),	//	Tom 3 R
	getPinLoc(A1	,	0),	//	Tom 4 T
	getPinLoc(A1	,	1),	//	Tom 4 R
	getPinLoc(A1	,	2),	//	Crash 1 T
	getPinLoc(A1	,	3),	//	Crash 1 R
	getPinLoc(A1	,	4),	//	Crash 2 T
	getPinLoc(A1	,	5),	//	Crash 2 R
	getPinLoc(A1	,	6),	//	Ride 1 T
	getPinLoc(A1	,	7),	//	Ride 1 R
	getPinLoc(A2	,	0),	//	Ride 1 S
	getPinLoc(A2	,	1),	//	Ride 2 T
	getPinLoc(A2	,	2),	//	Ride 2 R
	getPinLoc(A2	,	3),	//	Ride 2 S
	getPinLoc(A2	,	4),	//	Ride 3 T
	getPinLoc(A2	,	5),	//	Ride 3 R
	getPinLoc(A2	,	6),	//	Ride 3 S
	getPinLoc(A2	,	7),	//	Aux 1 T
	getPinLoc(A3	,	0),	//	Aux 1 R
	getPinLoc(A3	,	1),	//	Aux 2 T
	getPinLoc(A3	,	2),	//	Aux 2 R
	getPinLoc(A3	,	3),	//	Aux 3 T
	getPinLoc(A3	,	4),	//	Aux 3 R
	getPinLoc(A3	,	5),	//	Aux 4 T
	getPinLoc(A3	,	6),	//	Aux 4 R
	getPinLoc(A3	,	7),	//	Aux 5 T
	getPinLoc(A10	,	0),	//	Aux 5 R
};
byte pinType[MAX_PIN_SIZE];
byte pinThreshold[MAX_PIN_SIZE];
byte pinNoteOnThreshold[MAX_PIN_SIZE];
byte pinNoteOnValue[MAX_PIN_SIZE];
byte pinPitch[MAX_PIN_SIZE];
#pragma endregion

#pragma region Setups
void setup() {
	EEPROM_Load();
	setupADC();
	setupSerial();
}
// set up funcs
void setupADC() {
	// Define various ADC prescaler
	//------------------------------
	const unsigned char PS_16 = (1 << ADPS2);
	//const unsigned char PS_32 = (1 << ADPS2) | (1 << ADPS0);
	//const unsigned char PS_64 = (1 << ADPS2) | (1 << ADPS1);
	const unsigned char PS_128 = (1 << ADPS2) | (1 << ADPS1) | (1 << ADPS0);

	//http://www.microsmart.co.za/technical/2014/03/01/advanced-arduino-adc/
	ADCSRA &= ~PS_128;  // remove bits set by Arduino library
	ADCSRA |= PS_16;    // set our own prescaler to 16 for ~20 us
}
#pragma endregion

#pragma region Serial
#define BAUD_RATE 115200
#define MAX_DATA_BYTES          7 // max number of data bytes in incoming messages
#define START_SYSEX             0xF0 // start a MIDI Sysex message
#define END_SYSEX               0xF7 // end a MIDI Sysex message

byte storedInputData[MAX_DATA_BYTES]; // multi-byte data for SysEx
int sysexBytesRead = 0;
boolean parsingSysex = false;

void setupSerial() {
	Serial.begin(BAUD_RATE);
	while (!Serial);
	Serial.print("Welcome to aDrums v");
	Serial.print(VersionMajor);
	Serial.print(".");
	Serial.print(VersionMinor);
}

void processSerialInput() {
	while (Serial.available()) {
		int inputData = Serial.read(); // this is 'int' to handle -1 when no data
		if (inputData != -1) {
			parse(inputData);
		}
	}
}
void parse(byte inputData) {
	if (parsingSysex)
	{
		if (inputData == END_SYSEX)
		{
			//stop sysex byte
			parsingSysex = false;
			//fire off handler function
			processSysexMessage();
		}
		else
		{
			//normal data byte - add to buffer
			storedInputData[sysexBytesRead] = inputData;
			sysexBytesRead++;
		}
	}
	else if (inputData == START_SYSEX)
	{
		parsingSysex = true;
		sysexBytesRead = 0;
	}
}
void processSysexMessage() {
	sysexCallback(storedInputData[0], sysexBytesRead - 1, storedInputData + 1);
}

void TX_SERIAL(byte command, byte pin, int value) {
	Serial.write(START_SYSEX);
	Serial.write(command);
	Serial.write(pin);
	Serial.write(value);
	Serial.write(END_SYSEX);
	Serial.flush();
}
#pragma endregion

#pragma region Loops
void loop()
{
	processSerialInput();
	readPins();
}
#pragma endregion

#pragma region MIDI
#define DEFAULT_CHANNEL 0
// First parameter is the event type (0x09 = note on, 0x08 = note off).
// Second parameter is note-on/note-off, combined with the channel.
// Channel can be anything between 0-15. Typically reported to the user as 1-16.
// Third parameter is the note number (48 = middle C).
// Fourth parameter is the velocity (64 = normal, 127 = fastest).
void noteOn(byte channel, byte pitch, byte velocity) {
	midiEventPacket_t noteOn = { 0x09, 0x90 | channel, pitch, velocity };
	MidiUSB.sendMIDI(noteOn);
}
void noteOff(byte channel, byte pitch, byte velocity) {
	midiEventPacket_t noteOff = { 0x08, 0x80 | channel, pitch, velocity };
	MidiUSB.sendMIDI(noteOff);
}
// First parameter is the event type (0x0B = control change).
// Second parameter is the event type, combined with the channel.
// Third parameter is the control number number (0-119).
// Fourth parameter is the control value (0-127).
void controlChange(byte channel, byte control, byte value) {
	midiEventPacket_t event = { 0x0B, 0xB0 | channel, control, value };
	MidiUSB.sendMIDI(event);
}
#pragma endregion

#pragma region ReadDrums
void readPins() {
	for (byte i = 0; i < MAX_PIN_SIZE; i++)
	{
		switch (pinType[i])
		{
		case PINTYPE_DISABLED:
			break;
		default:
			ReadDrum(i, getAnalogueValue(i));
			break;
		}
	}
}
int getAnalogueValue(byte pin) {
	byte location = pinLocation[pin];
	digitalWrite(ADCD1, (location & FLAG_IC1) ? HIGH : LOW);  //flag 00000001
	digitalWrite(ADCD2, (location & FLAG_IC2) ? HIGH : LOW);  //flag 00000010
	digitalWrite(ADCD3, (location & FLAG_IC3) ? HIGH : LOW);  //flag 00000100
	return	analogRead((location >> 3));
}
void ReadDrum(byte pin, int value) {
	if (value >= pinThreshold[pin])
	{
		if (pinNoteOnValue[pin] == 0)
			noteOn(DEFAULT_CHANNEL, pinPitch[pin], getVelocity(pin, value));

		pinNoteOnValue[pin]++;
	}
	else if (pinNoteOnValue[pin] > 0 && pinNoteOnValue[pin] > pinNoteOnThreshold[pin])
	{
		noteOff(DEFAULT_CHANNEL, pinPitch[pin], 0);
		pinNoteOnValue[pin] = 0;
	}
}
byte getVelocity(byte pin, int analogue_value) {
	//analogue_value max = 1023
	return (analogue_value / 8);
}
#pragma endregion

#pragma region Callbacks
#define toMSG(NAME) MSG_ ## NAME
#define caseCallBack(NAME) \
	case toMSG(NAME): \
		if(isSet) \
			  NAME [arrayPointer[0]] = arrayPointer[1]; \
		else \
			TX_SERIAL( command , arrayPointer[0], NAME [arrayPointer[0]] ); \
	break; 

#define MSG_GET_HANDSHAKE 0
#define MSG_GET_PINCOUNT 8
#define MSG_EEPROM 100
#define MSG_pinType 1
#define MSG_pinThreshold 2
#define MSG_pinNoteOnThreshold 3
#define MSG_pinPitch 4

void sysexCallback(byte command, byte size, byte* arrayPointer) {
	bool isSet = command & 1;
	switch ((command >> 1))
	{
	case MSG_GET_HANDSHAKE: TX_SERIAL(command, VersionMajor, VersionMinor); break;
	case MSG_GET_PINCOUNT: TX_SERIAL(command, MAX_PIN_SIZE, 0); break; 
	case MSG_EEPROM: if (isSet) EEPROM_Save(); else EEPROM_Load(); TX_SERIAL(command, 1, 1); break;
		caseCallBack(pinType);
		caseCallBack(pinThreshold);
		caseCallBack(pinNoteOnThreshold);
		caseCallBack(pinPitch);
	}
}

#pragma endregion

#pragma region EEPROM
#define _S_EPROM(a,b) eeprom_write_bytes((MAX_PIN_SIZE * b), a, MAX_PIN_SIZE)
#define _L_EPROM(a,b) eeprom_read_bytes((MAX_PIN_SIZE * b), a, MAX_PIN_SIZE)
#define _EPROM_StartAddr 0

void EEPROM_Save() {
	_S_EPROM(pinType, 0);
	_S_EPROM(pinThreshold, 1);
	_S_EPROM(pinNoteOnThreshold, 2);
	_S_EPROM(pinPitch, 3);
}
void EEPROM_Load() {
	_L_EPROM(pinType, 0);
	_L_EPROM(pinThreshold, 1);
	_L_EPROM(pinNoteOnThreshold, 2);
	_L_EPROM(pinPitch, 3);
}

//
// Absolute min and max eeprom addresses.
// Actual values are hardware-dependent.
//
// These values can be changed e.g. to protect
// eeprom cells outside this range.
//
const int EEPROM_MIN_ADDR = 0;
const int EEPROM_MAX_ADDR = 1023;

//
// Returns true if the address is between the
// minimum and maximum allowed values,
// false otherwise.
//
// This function is used by the other, higher-level functions
// to prevent bugs and runtime errors due to invalid addresses.
//
boolean eeprom_is_addr_ok(int addr) {
	return ((addr >= EEPROM_MIN_ADDR) && (addr <= EEPROM_MAX_ADDR));
}

//
// Writes a sequence of bytes to eeprom starting at the specified address.
// Returns true if the whole array is successfully written.
// Returns false if the start or end addresses aren't between
// the minimum and maximum allowed values.
// When returning false, nothing gets written to eeprom.
//
boolean eeprom_write_bytes(int startAddr, const byte* array, int numBytes) {
	// counter
	int i;

	// both first byte and last byte addresses must fall within
	// the allowed range  
	if (!eeprom_is_addr_ok(startAddr) || !eeprom_is_addr_ok(startAddr + numBytes)) {
		return false;
	}

	for (i = 0; i < numBytes; i++) {
		EEPROM.write(startAddr + i, array[i]);
	}

	return true;
}

//
// Reads the specified number of bytes from the specified address into the provided buffer.
// Returns true if all the bytes are successfully read.
// Returns false if the star or end addresses aren't between
// the minimum and maximum allowed values.
// When returning false, the provided array is untouched.
//
// Note: the caller must ensure that array[] has enough space
// to store at most numBytes bytes.
//
boolean eeprom_read_bytes(int startAddr, byte array[], int numBytes) {
	int i;

	// both first byte and last byte addresses must fall within
	// the allowed range  
	if (!eeprom_is_addr_ok(startAddr) || !eeprom_is_addr_ok(startAddr + numBytes)) {
		return false;
	}

	for (i = 0; i < numBytes; i++) {
		array[i] = EEPROM.read(startAddr + i);
	}

	return true;
}
#pragma endregion

