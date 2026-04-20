package application;

import javafx.animation.KeyFrame;
import javafx.animation.Timeline;
import javafx.beans.property.DoubleProperty;
import javafx.scene.layout.Pane;
import javafx.scene.paint.Color;
import javafx.scene.shape.Circle;
import javafx.util.Duration;

public class BallPane extends Pane {
    public final double radius = 10;
    private double x = radius, y = radius;
    private double dx = 1, dy = 1;
    private Circle circle = new Circle(x, y, radius);
    private Timeline animation;
    GameManager gameManager;

    public BallPane(GameManager game) {
        circle.setFill(Color.RED); // Set ball color
        getChildren().add(circle); // Place a ball into this pane
        gameManager = game;
        
        // Create an animation for moving the ball
        animation = new Timeline(
            new KeyFrame(Duration.millis(50), e -> moveBall()));
        animation.setCycleCount(Timeline.INDEFINITE);
        animation.play(); // Start animation
    	animation.setRate(animation.getRate() + 1.5); //set speed
    }
    public void play() {
        animation.play();
    }

    public void pause() {
        animation.pause();
    }

    public void increaseSpeed() {
        animation.setRate(animation.getRate() + 0.5);
    }

    public void decreaseSpeed() {
        animation.setRate(
            animation.getRate() > 0 ? animation.getRate() - 0.1 : 0);
    }

    public DoubleProperty rateProperty() {
        return animation.rateProperty();
    }

    
    boolean paddleCollsion(Paddle paddle) { //refrenced in collborations
    	double paddleX = paddle.getX();
    	double paddleRight = paddleX + paddle.getWidth();
    	double paddleTop = paddle.getY();
    	double paddleBottom = paddleTop + paddle.getHeight();
    	
    	  return (x - radius) <= paddleRight //returns if ball overlaps or collides
    	            && (x - radius) >= paddleX
    	            && y >= paddleTop
    	            && y <= paddleBottom;
    }
    protected void moveBall() {
        // Check boundaries
        if (x > getWidth() - radius || paddleCollsion(gameManager.paddle) ) {
        	if( paddleCollsion(gameManager.paddle)) {
                gameManager.addScore(); //increase score when the ball bounces
        	}
        	if(gameManager.getScore()%2 == 0) { //this adds impact to the ball bouncing 
        		circle.setFill(Color.BLUE);
        	}else {
        		circle.setFill(Color.RED);
        	}
            dx *= -1; // Change ball move direction
            increaseSpeed(); //increase speed when a wall is hit
        }
        else if(x < radius) {
        	gameManager.gameOver(this);
        	circle.setFill(null); 
        	
        }
        if (y < radius || y > getHeight() - radius) {
            dy *= -1; // Change ball move direction
        }
        // Adjust ball position
        x += dx;
        y += dy;
        circle.setCenterX(x);
        circle.setCenterY(y);
    }
}
