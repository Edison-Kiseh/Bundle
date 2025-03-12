import UsernameInput from '../components/UsernameInput';

export default function Home() {
  return (
    <div className="containerWelcome">
      <h1 className="title">Welcome to the Avanade Support Hub</h1>
      <p className="description">Please enter your username to continue</p>
      <UsernameInput />
    </div>
  );
}