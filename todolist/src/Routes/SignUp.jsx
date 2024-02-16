import React from 'react';
import SignUpForm from '../components/SignUpForm';
import logo from '../assets/logo.png';
import { Link } from 'react-router-dom';

function SignUp() {
    return (
        <div className='bodyForm'>
            <img src={logo} style={{height:100, marginBottom:30,marginTop:10}} className="logo react" alt="React logo" />
            <SignUpForm />
            <p>Already have an account? <Link to="/login">Login</Link></p>
        </div>
    );
}

export default SignUp;
