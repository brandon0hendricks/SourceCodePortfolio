package Cop2251.fall25.week1.hendricks;
//Student ID: 2438173
//Brandon Hendricks
public class CheckingAccount extends Account {

	int numOfChecks;
	
	CheckingAccount(String accountNumber){
		this (accountNumber,0); //uses Account constructor with starting balance of zero
	}
	CheckingAccount(String accountNumber,double balance){ //two arg-constructor
		super(accountNumber,balance); //uses Account constructor with starting balance
	}
	
	public void withdraw(double amount) { //takes from balance
		if(numOfChecks > 3) {
			super.withdraw(amount + 3); //withdraws with fee
		}else {
			super.withdraw(amount);
		}
	}
	
	
}
