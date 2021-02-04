CXX=g++
CXXFLAGS=-std=c++17 -Ofast -g -Wall -Wextra

SFML=$(shell pkg-config sfml-system   sfml-audio --libs --cflags || echo '-lsfml-audio -lsfml-system')

all: engsyn;

engsyn: engsyn.cc
	$(CXX) -o $@ $^ $(CXXFLAGS) $(CPPFLAGS) $(LDLIBS) $(SFML)

clean:	;
	rm -f $(TARGETS)
