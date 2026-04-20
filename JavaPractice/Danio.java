package Cop2251.fall25.week1.hendricks;

public class Danio extends Fish {

	Danio(){ //no-arg constructor
		super(1); //uses super constructor to create fish
	}

	public String swim() {
		return  "Danio darting"; //swim type
	}

	public int getOxygenConsumption() {
		return 12;
	}

}
