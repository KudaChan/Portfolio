import Navbar from "./components/Navbar";
import Hero from "./components/Hero";
import Highlights from "./components/Highlights";
import Model from "./components/Model";

import * as Sentry from "@sentry/react";

const App = () => {
  // const transaction = Sentry.startTransaction({ name: "test-transaction" });
  // const span = transaction.startChild({ op: "functionX" }); // This function returns a Span
  // // functionCallX
  // span.finish(); // Remember that only finished spans will be sent with the transaction
  // transaction.finish(); // Finishing the transaction will send it to Sentry
  return <button onClick={() => methodDoesNotExist()}>Break the world</button>;

  return (
    <main className="bg-black">
      <Navbar />
      <Hero />
      <Highlights />
      <Model />
    </main>
  )
}

export default Sentry.withProfiler(App);
