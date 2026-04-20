package Cop2251.fall25.week1.hendricks;
//Student ID: 2438173
//Brandon Hendricks
public class SavingsAccount extends Account {
	
	double minBalance = 100;
	SavingsAccount(String accountNumber){ //one-arg constructor
		this(accountNumber,0); //uses Account constructor with starting balance of zero
	}
	SavingsAccount(String accountNumber,double balance){ //two arg-constructor
		super(accountNumber,balance); //uses Account constructor with starting balance
	}
	
	public void addInterest() { //calculates and adds interest, should override super
		if(getBalance() >= minBalance ) {
			super.addInterest();
		}
	}
}
